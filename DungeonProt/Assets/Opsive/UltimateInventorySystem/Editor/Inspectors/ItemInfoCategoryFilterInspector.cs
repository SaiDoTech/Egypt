﻿namespace Opsive.UltimateInventorySystem.Editor.Inspectors
{
    using Opsive.Shared.Editor.Utility;
    using Opsive.UltimateInventorySystem.Crafting;
    using Opsive.UltimateInventorySystem.Editor.VisualElements;
    using Opsive.UltimateInventorySystem.UI.Menus;
    using Opsive.UltimateInventorySystem.UI.Panels.ItemViewSlotContainers.GridFilterSorters.InventoryGridFilters;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine.UIElements;

    /// <summary>
    /// Custom editor to display the category item actions.
    /// </summary>
    [CustomEditor(typeof(ItemInfoCategoryFilter), true)]
    public class ItemInfoCategoryFilterInspector : DatabaseInspectorBase
    {
        protected override List<string> PropertiesToExclude => new List<string>() { "m_ShowItemCategory", "m_HideCategory"  };

        protected ItemInfoCategoryFilter m_ItemInfoCategoryFilter;
        protected ItemCategoryField m_ShowCategory;
        protected ItemCategoryField m_HideCategory;

        /// <summary>
        /// Initialize the inspector when it is first selected.
        /// </summary>
        protected override void InitializeInspector()
        {
            m_ItemInfoCategoryFilter = target as ItemInfoCategoryFilter;

            base.InitializeInspector();
        }

        /// <summary>
        /// Create the inspector.
        /// </summary>
        /// <param name="container">The parent container.</param>
        protected override void CreateInspector(VisualElement container)
        {
            m_ShowCategory = new ItemCategoryField(
                "Show Item Category",
                m_Database,
                (newValue)=>
                {
                    m_ItemInfoCategoryFilter.ShowItemCategory = newValue;
                    InspectorUtility.SetDirty(m_ItemInfoCategoryFilter);
                    m_ShowCategory.Refresh(m_ItemInfoCategoryFilter.ShowItemCategory);
                },null);
            m_ShowCategory.Refresh(m_ItemInfoCategoryFilter.ShowItemCategory);
            
            container.Add(m_ShowCategory);

            m_HideCategory = new ItemCategoryField(
                "Hide Item Category",
                m_Database,
                (newValue)=>
                {
                    m_ItemInfoCategoryFilter.HideCategory = newValue;
                    InspectorUtility.SetDirty(m_ItemInfoCategoryFilter);
                    m_HideCategory.Refresh(m_ItemInfoCategoryFilter.HideCategory);
                },null);
            m_HideCategory.Refresh(m_ItemInfoCategoryFilter.HideCategory);
            
            container.Add(m_HideCategory);
        }
    }
}