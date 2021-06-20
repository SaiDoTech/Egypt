﻿/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Menus.Shop
{
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.Core.InventoryCollections;
    using Opsive.UltimateInventorySystem.Exchange;
    using Opsive.UltimateInventorySystem.Exchange.Shops;
    using Opsive.UltimateInventorySystem.UI.Currency;
    using Opsive.UltimateInventorySystem.UI.Item;
    using Opsive.UltimateInventorySystem.UI.Item.ItemViewModules;
    using Opsive.UltimateInventorySystem.UI.Panels;
    using Opsive.UltimateInventorySystem.UI.Panels.ItemViewSlotContainers;
    using Opsive.UltimateInventorySystem.UI.Views;
    using UnityEngine;
    using UnityEngine.UI;
    using Text = Opsive.UltimateInventorySystem.UI.CompoundElements.Text;

    /// <summary>
    /// Shop menu, used to display and interact with a shop component
    /// </summary>
    public class ShopMenu : InventoryPanelBinding
    {
        [Tooltip("The shop component to show in the UI")]
        [SerializeField] internal ShopBase m_Shop;
        [Tooltip("The inventory grid UI.")]
        [SerializeField] internal InventoryGrid m_InventoryGrid;
        [Tooltip("The currency UI displaying the total price.")]
        [SerializeField] internal MultiCurrencyView m_TotalPrice;
        [Tooltip("The quantity picker panel.")]
        [SerializeField] internal QuantityPickerPanel m_QuantityPickerPanel;
        [Tooltip("Limit the quantity picker max value to the available item amount count.")]
        [SerializeField] protected bool m_LimitBuyQuantityToAvailableItemAmount;
        [Tooltip("Buy or Sell an item on item click.")]
        [SerializeField] protected bool m_OpenQuantityPickerOnItemClick = true;
        [Tooltip("The menu title text.")]
        [SerializeField] protected Text m_MenuTitle;
        [Tooltip("The buy button.")]
        [SerializeField] protected Button m_BuyButton;
        [Tooltip("The sell button.")]
        [SerializeField] protected Button m_SellButton;
        [Tooltip("The close menu button.")]
        [SerializeField] protected Button m_CloseButton;
        [Tooltip("The buy modifier text.")]
        [SerializeField] protected Text m_BuyModifierText;
        [Tooltip("The sell modifier text.")]
        [SerializeField] protected Text m_SellModifierText;

        protected bool m_IsShopSet;

        protected bool m_IsBuying;

        protected CurrencyCollection m_TempCurrencyCollection;
        private ICurrencyOwner<CurrencyCollection> m_ShopperClientCurrencyOwner;

        protected ItemInfo m_SelectedItemInfo;

        public ShopBase Shop => m_Shop;
        public InventoryGrid InventoryGrid => m_InventoryGrid;

        public bool IsBuying => m_IsBuying;
        public ItemInfo SelectedItemInfo => m_SelectedItemInfo;

        /// <summary>
        /// Set up the panel.
        /// </summary>
        public override void Initialize(DisplayPanel display)
        {
            if(m_IsInitialized){return;}
            base.Initialize(display);

            if (m_BuyButton != null) {
                m_BuyButton.onClick.RemoveAllListeners();
                m_BuyButton.onClick.AddListener(OpenBuy);
            }
            
            if (m_SellButton != null) {
                m_SellButton.onClick.RemoveAllListeners();
                m_SellButton.onClick.AddListener(OpenSell);
            }

            if (m_CloseButton != null) {
                m_CloseButton.onClick.RemoveAllListeners();
                m_CloseButton.onClick.AddListener(() => m_DisplayPanel.Close(true));
            }
            
            m_TempCurrencyCollection = new CurrencyCollection();

            m_InventoryGrid.Initialize(false);

            m_InventoryGrid.Grid.ViewDrawer.AfterDrawing += DrawBuySellPrice;

            m_InventoryGrid.OnItemViewSlotClicked += OnItemClicked;
            m_InventoryGrid.OnItemViewSlotSelected += OnItemSelected;

            m_QuantityPickerPanel.OnAmountChanged += QuantityPickerAmountChanged;
            m_QuantityPickerPanel.ConfirmCancelPanel.OnConfirm += BuySellItem;

            SetShop(m_Shop);
        }

        /// <summary>
        /// Set the shop.
        /// </summary>
        /// <param name="shop">The shop.</param>
        public virtual void SetShop(ShopBase shop)
        {
            if (shop == null) { return; }
            if (m_IsShopSet && m_Shop == shop) { return; }

            m_IsShopSet = true;
            
            m_Shop = shop;

            SetModifierUIs();
        }

        protected override void OnInventoryBound()
        {
            m_ShopperClientCurrencyOwner = m_Inventory.GetCurrencyComponent<CurrencyCollection>();
            if (m_ShopperClientCurrencyOwner == null) {
                Debug.LogWarning("The inventory bound to the shop menu does not have a currency owner.");
            }
            SetModifierUIs();
        }
        
        public override void OnOpen()
        {
            base.OnOpen();
            OpenBuy();
        }

        /// <summary>
        /// Open the buy sub menu.
        /// </summary>
        private void OpenBuy()
        {
            m_MenuTitle.text = "SHOP - BUY";
            
            m_InventoryGrid.Panel.Open(m_DisplayPanel,m_BuyButton);
            m_IsBuying = true;
            m_InventoryGrid.SetInventory(m_Shop.Inventory);
            m_InventoryGrid.Draw();
            m_InventoryGrid.SelectSlot(0);
        }

        /// <summary>
        /// Open the sell sub menu.
        /// </summary>
        private void OpenSell()
        {
            m_MenuTitle.text = "SHOP - SELL";
            
            m_InventoryGrid.Panel.Open(m_DisplayPanel,m_SellButton);
            m_IsBuying = false;
            m_InventoryGrid.SetInventory(m_Inventory);
            m_InventoryGrid.Draw();
            m_InventoryGrid.SelectSlot(0);
        }

        /// <summary>
        /// Get the price of an item amount.
        /// </summary>
        /// <param name="itemInfo">The item.</param>
        /// <returns>The currency collection.</returns>
        protected CurrencyCollection GetPrice(ItemInfo itemInfo)
        {
            if (itemInfo.Item == null) { return null; }
            var shop = (Shop)m_Shop;

            if (m_IsBuying) {
                if (shop.TryGetBuyValueForBuyer(m_Inventory, itemInfo, ref m_TempCurrencyCollection)) {
                    return m_TempCurrencyCollection;
                }
            } else {
                if (shop.TryGetSellValueForSeller(m_Inventory, itemInfo, ref m_TempCurrencyCollection)) {
                    return m_TempCurrencyCollection;
                }
            }

            return null;
        }

        /// <summary>
        /// Draw the price for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="multiCurrencyView">The currency UI.</param>
        protected void DrawPriceTo(ItemInfo itemInfo, MultiCurrencyView multiCurrencyView)
        {
            var price = GetPrice(itemInfo);

            if (price == null) {
                multiCurrencyView.DrawEmptyCurrency();
                return;

            }
            multiCurrencyView.DrawCurrency(m_TempCurrencyCollection);
        }

        /// <summary>
        /// The quantity picker changed value.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        protected void QuantityPickerAmountChanged(int quantity)
        {
            DrawPriceTo((quantity, m_SelectedItemInfo), m_TotalPrice);
        }

        /// <summary>
        /// Draw buy or sell price.
        /// </summary>
        /// <param name="itemView">The Item View ui.</param>
        /// <param name="itemInfo">The item info.</param>
        protected virtual void DrawBuySellPrice(View<ItemInfo> itemView, ItemInfo itemInfo)
        {
            if (m_IsBuying) {
                DrawBuyPrice(itemView, itemInfo);
            } else {
                DrawSellPrice(itemView, itemInfo);
            }
        }

        /// <summary>
        /// Draw the buy price.
        /// </summary>
        /// <param name="itemView">The Item View ui.</param>
        /// <param name="itemInfo">The item info.</param>
        protected virtual void DrawBuyPrice(View<ItemInfo> itemView, ItemInfo itemInfo)
        {
            var currencyUI = FindItemCurrencyUI(itemView);
            if (currencyUI == null) { return; }

            DrawPriceTo((1, itemInfo), currencyUI);
        }

        /// <summary>
        /// Draw the sell price.
        /// </summary>
        /// <param name="itemView">The Item View ui.</param>
        /// <param name="itemInfo">The item info.</param>
        protected virtual void DrawSellPrice(View<ItemInfo> itemView, ItemInfo itemInfo)
        {
            var currencyUI = FindItemCurrencyUI(itemView);
            if (currencyUI == null) { return; }

            DrawPriceTo((1, itemInfo), currencyUI);
        }

        /// <summary>
        /// Find the currency UI on the Item View.
        /// </summary>
        /// <param name="itemView">The Item View UI.</param>
        /// <returns>The currency UI.</returns>
        protected virtual MultiCurrencyView FindItemCurrencyUI(View<ItemInfo> itemView)
        {
            for (int i = 0; i < itemView.Modules.Count; i++) {
                if (itemView.Modules[i] is CurrencyItemView currencyComponent) {
                    return currencyComponent.MultiCurrencyView;
                }
            }

            return null;
        }

        /// <summary>
        /// The event when selecting an item.
        /// </summary>
        /// <param name="inventoryGrid">The inventory grid UI.</param>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="index">The index.</param>
        protected virtual void OnItemSelected(ItemViewSlotEventData slotEventData)
        {
            if (m_ShopperClientCurrencyOwner == null || m_Inventory == null) {
                Debug.LogError("The client inventory is either null or it does not have a currencyOwner.",gameObject);
                return;
            }
            var itemInfo = slotEventData.ItemViewSlot.ItemInfo;
            
            if (m_QuantityPickerPanel.IsOpen == false) { return; }
            if (m_SelectedItemInfo.Item == itemInfo.Item) { return; }

            m_SelectedItemInfo = itemInfo;
            m_QuantityPickerPanel.SetPreviousSelectable(slotEventData.ItemViewSlot);

            m_QuantityPickerPanel.QuantityPicker.MinQuantity = 1;
            if (m_IsBuying) {
                var price = GetPrice((1, itemInfo));
                var quotient = m_ShopperClientCurrencyOwner.PotentialQuotientFor(price);

                var max = Mathf.Max(quotient, 1);
                if (m_LimitBuyQuantityToAvailableItemAmount) {
                    max = Mathf.Min(max, itemInfo.Amount);
                }
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = max;

                m_QuantityPickerPanel.ConfirmCancelPanel.EnableConfirm(quotient > 0);
            } else {
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = itemInfo.Amount;
            }

            m_QuantityPickerPanel.QuantityPicker.SetQuantity(1);

            var quantity = m_QuantityPickerPanel.QuantityPicker.Quantity;

            DrawPriceTo((quantity, m_SelectedItemInfo), m_TotalPrice);
        }

        /// <summary>
        /// The event when clicking an item.
        /// </summary>
        /// <param name="inventoryGrid">The inventory grid UI.</param>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="index">The index.</param>
        protected virtual void OnItemClicked(ItemViewSlotEventData slotEventData)
        {
            if(m_OpenQuantityPickerOnItemClick == false){ return; }
            
            var itemInfo = slotEventData.ItemViewSlot.ItemInfo;
            
            m_SelectedItemInfo = itemInfo;
            m_QuantityPickerPanel.Open(m_InventoryGrid.Panel, slotEventData.ItemViewSlot);

            m_QuantityPickerPanel.QuantityPicker.MinQuantity = 1;
            if (m_IsBuying) {
                var price = GetPrice((1, itemInfo));
                var quotient = m_ShopperClientCurrencyOwner.PotentialQuotientFor(price);

                var max = Mathf.Max(quotient, 1);
                if (m_LimitBuyQuantityToAvailableItemAmount) {
                    max = Mathf.Min(max, itemInfo.Amount);
                }
                
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = max;

                m_QuantityPickerPanel.ConfirmCancelPanel.EnableConfirm(quotient > 0);
                m_QuantityPickerPanel.ConfirmCancelPanel.SetConfirmText("Buy");
            } else {
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = itemInfo.Amount;

                m_QuantityPickerPanel.ConfirmCancelPanel.EnableConfirm(true);
                m_QuantityPickerPanel.ConfirmCancelPanel.SetConfirmText("Sell");
            }

            m_QuantityPickerPanel.QuantityPicker.SetQuantity(1);

            var quantity = m_QuantityPickerPanel.QuantityPicker.Quantity;

            DrawPriceTo((quantity, m_SelectedItemInfo), m_TotalPrice);
        }

        /// <summary>
        /// Buy or sell an item once the player confirmed the quantity. 
        /// </summary>
        private void BuySellItem()
        {
            var quantity = m_QuantityPickerPanel.QuantityPicker.Quantity;

            if (quantity < 1) { return; }

            if (m_IsBuying) {
                m_Shop.BuyItem(m_Inventory, m_ShopperClientCurrencyOwner, (quantity, m_SelectedItemInfo));
            } else {
                m_Shop.SellItem(m_Inventory, m_ShopperClientCurrencyOwner, (quantity, m_SelectedItemInfo));
            }

            m_InventoryGrid.Draw();
        }

        /// <summary>
        /// Display the modifier values
        /// </summary>
        private void SetModifierUIs()
        {
            if (m_Shop == null || m_Inventory == null) {
                return;
            }

            var buyModifier = m_Shop.GetBuyModifierForBuyer(m_Inventory);
            var sellModifier = m_Shop.GetSellModifierForSeller(m_Inventory);

            var buyPecDiff = Mathf.RoundToInt((buyModifier - 1) * 100);
            var sellPecDiff = Mathf.RoundToInt((sellModifier - 1) * 100);

            var buySign = buyPecDiff <= 0 ? "" : "+";
            var sellSign = sellPecDiff <= 0 ? "" : "+";

            m_BuyModifierText.text = $"Buy Modifer: {buySign}{buyPecDiff}%";
            m_SellModifierText.text = $"Sell Modifer: {sellSign}{sellPecDiff}%";
        }

        
    }
}
