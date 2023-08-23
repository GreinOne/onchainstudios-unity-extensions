//*****************************************************************************
// Author: Rie Kumar
// Copyright: cryptoys, 2023
//*****************************************************************************

using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace OnChainStudios.UIToolkitExtensions
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// [INSERT CLASS COMMENT HERE]
    /// </summary>
    public class ListViewEventBusBridge : MonoBehaviour
    {
        /// <summary>
        /// Name of the event posted to the <see cref="EventBus"/> when a <see cref="BindItemEvent"/> is triggered.
        /// </summary>
        public static string BindItemEvent => $"{typeof(ListViewEventBusBridge).FullName}.{nameof(BindItemEvent)}";
        
        /// <summary>
        /// Name of the event posted to the <see cref="EventBus"/> when a <see cref="UnbindItemEvent"/> is triggered.
        /// </summary>
        public static string UnbindItemEvent => $"{typeof(ListViewEventBusBridge).FullName}.{nameof(UnbindItemEvent)}";
        
        public string VisualElementName;

        public VisualTreeAsset VisualTreeAsset;

        /// <summary>
        /// Handle to the <see cref="UIDocument"/>.
        /// </summary>
        protected UIDocument UIDocument;
        
        /// <inheritdoc/>
        protected virtual void Awake()
        {
            UIDocument = GetComponent<UIDocument>();
        }
        
        /// <inheritdoc/>
        protected virtual void OnEnable()
        {
            if (UIDocument != null && UIDocument.rootVisualElement != null)
            {
                var listView = UIDocument.rootVisualElement.Q<ListView>(VisualElementName);
                
                RegisterCallbacks(listView);
            }
        }

        /// <summary>
        /// Registers callbacks on the <paramref name="visualElement"/>.
        /// </summary>
        /// <param name="visualElement">The handle to the <see cref="VisualElement"/> you want to register the events for.</param>
        protected virtual void RegisterCallbacks(ListView listView)
        {
            listView.makeItem = () =>
            {
                return VisualTreeAsset.Instantiate();
            };

            listView.bindItem = (item, index) =>
            {
                EventBus.Trigger(BindItemEvent, new ListViewBindItemEventArgs(listView, item, index));
            };
            
            listView.unbindItem = (item, index) =>
            {
                EventBus.Trigger(UnbindItemEvent, new ListViewUnbindItemEventArgs(listView, item, index));
            };
        }
    }
}
