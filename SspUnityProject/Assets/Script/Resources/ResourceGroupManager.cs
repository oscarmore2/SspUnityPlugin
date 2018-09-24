using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
	public class ResourceGroupManager : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
    {
        public Transform ResourceGroupContainor;

		private ResourcesManager resourceManager;

		public ISelectableItem CurrentSelected{ get; private set;}

		public ResourcesManager ResourceManager
		{
			get{ return resourceManager; }
		}

        [SerializeField]
        ViewVertiheightCalculator heightCalculator;

        public void OnInit(ResourcesManager resManager)
        {
            resourceManager = resManager;
			var config = new JsonConfiguration (Paths.RESOURCE_GROUP);
			ResourceGroupList.Instance.Init (this);
			ResourceGroupList.Instance.LoadConfig (config.Data);
            gameObject.SetActive(true);
        }

        public void OnAddToGroup()
        {
            var toggles = resourceManager.TaggleGroup.ActiveToggles();
			int index = 1;
            
			LitJson.JsonData data = new LitJson.JsonData ();
			data ["index"] = ResourceGroupList.Instance.Count + 1;
			data ["data"] = new LitJson.JsonData ();
			data ["data"]["Name"] = "Group" + ResourceGroupList.Instance.Count + 1;
			data ["data"]["Priority"] = ResourceGroupList.Instance.Count + 1;
			data["data"]["IsAfterTransition"] = false;
			data["data"]["Scale"] = 1;
			data["data"]["XAxis"] = 0;
			data["data"]["YAxis"] = 0;
			data["data"]["Duration"] = 0;
			data["data"]["ActivateState"] = new LitJson.JsonData();
			for (int i = 0; i < 3; i++)
			{
				data["data"]["ActivateState"].Add(false);
			}
			data ["data"]["VCamBiding"] = new LitJson.JsonData();
			for (int i = 0; i < 8; i++)
			{
				data["data"]["VCamBiding"].Add(false);
			}
			data["data"]["ResourceRefs"] = new LitJson.JsonData();
			foreach (var t in toggles)
			{
				var comp = t.GetComponentInParent<ResourceItem>();
				LitJson.JsonData item = new LitJson.JsonData ();
				item ["GUID"] = comp.Resource.GUID;
				data ["data"] ["ResourceRefs"].Add(item);
				t.isOn = false;
			}
			if (data ["data"] ["ResourceRefs"].Count > 0) {
				ResourceGroupList.Instance.SetConfig (data);
				AddItem (ResourceGroupList.Instance.GetLast ());
				RefreshUI ();
			}
        }

		public void RefreshUI()
		{
			gameObject.SetActive (false);
			gameObject.SetActive (true);
		}

        void OnEnable()
        {
            if (ResourceGroupContainor == null || ResourceGroupList.Instance == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (ResourceGroupContainor.childCount < ResourceGroupList.Instance.Count)
            {
				var list = ResourceGroupList.Instance.Sort ();
				for (int i = 0; i < list.Count; i++)
                {
					AddItem(list[i]);
                }
				//heightCalculator.Calculate();
            }
        }

        void AddItem(ResourceGroup rg)
        {
            var o = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/ResourceGroupItem"));
            var rect = o.GetComponent<Transform>();
            rect.parent = ResourceGroupContainor;
			o.GetComponent<ResourceGroupItem>().SetContent(rg, this);
        }

		public void OnChangeResourceSelection(ISelectableItem Selectable)
		{
			if (CurrentSelected != null) {
				CurrentSelected.OnDeselect ();
				if (CurrentSelected == Selectable) {
					CurrentSelected = null;
					return;
				}
			}

			if (Selectable != null) {
				Selectable.OnSelect ();	
				CurrentSelected = Selectable;
			}
		}

		public void OnPointerClick(UnityEngine.EventSystems.PointerEventData data)
		{
			var list = data.pointerCurrentRaycast.gameObject.transform.GetComponentsInParent<ResourceGroupManager>();
			if (list [0] != this) {
				OnChangeResourceSelection (null);
			}
		}

		public void DeleteItem()
		{
			if (CurrentSelected != null) {
				var rg = (ResourceGroupItem)CurrentSelected;
				int id = ResourceGroupList.Instance.RemoveItem (rg.ResGroup);
				CurrentSelected = null;
				var c = ResourceGroupContainor.GetChild (id);
				Destroy (c.gameObject);
			}
		}
    }


}