using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
	public class ResourceGroupItem : MonoBehaviour, ISelectableItem
    {

        [SerializeField]
        VerticalLayoutGroup groups;

        public ResourceGroup ResGroup;

        //public Text Title;
        public Toggle PVWToggle;
        public Toggle PGMToggle;
        public Toggle BinDiagToggle;

        public Text Name;

        public InputField Scale;
        public InputField XAxis;
        public InputField YAxis;
        public InputField Duration;
		public Button ApplyChange;

		public Color SelectedColor;

        public Transform ResContainor;
		public Transform LayerButton; 
		public Graphic BackgroundImage;

		Button SelectionPanel;

		Color originalColor;
		ResourceGroupManager manager;

		public void SetContent(ResourceGroup g, ResourceGroupManager rgm)
        {
			manager = rgm;
            Name.text = g.Name;
            SelectionPanel = BackgroundImage.GetComponent<Button> ();
			originalColor = BackgroundImage.color;
            ResContainor.parent = ResContainor;
            XAxis.text = g.XAxis.ToString();
            YAxis.text = g.YAxis.ToString();
            Scale.text = g.Scale.ToString();

			Scale.onEndEdit.AddListener (FinishEditingScale);
			XAxis.onEndEdit.AddListener (FinishEditingXAxis);
			YAxis.onEndEdit.AddListener (FinishEditingYAxis);

            Duration.text = g.Duration.ToString();
            ResGroup = g;
			g.SortGroup ();
            for (int i = 0; i < g.ResourceRefs.Count; i++)
            {
                GameObject gameobj = null;
                switch (g.ResourceRefs[i].Resources.GetType())
                {
                    case ResourceType.Text:
                        gameobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/DetailBlockText"));
                        break;
                    case ResourceType.Image:
                        gameobj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/DetailBlockImage"));
                        break;
                    case ResourceType.Sequence:
                        //TODO: Set Sequence Content
                        break;
                    case ResourceType.Video:
                        //TODO: Set Video Content
                        break;
                    default:
                        //TODO: add default handle
                        break;
                }
                var objAttr = gameobj.GetComponent<ResourceAttributeFields>();
				objAttr.SetContent(g.ResourceRefs[i], this);
                objAttr.transform.parent = ResContainor;

                TransitionManager.Instance.OnTransitionEnd += (()=> {
                    if (!g.IsAfterTransition)
                    {
                        if (PVWToggle.isOn)
                        {
                            PVWToggle.isOn = false;
                            PGMToggle.isOn = true;
                        }
                        else if (PGMToggle.isOn) {
                            PGMToggle.isOn = false;
                            PVWToggle.isOn = true;
                        }
                    }
                });
            }

            PVWToggle.isOn = g.ActivateState[0];
            PGMToggle.isOn = g.ActivateState[1];
            BinDiagToggle.isOn = g.ActivateState[2];
        }

        public void OnSwitchToggle()
        {
            ResGroup.ActivateState = new bool[]{ PVWToggle.isOn, PGMToggle.isOn, BinDiagToggle.isOn};
            ResourceGroupList.Instance.OnResourceGroupChange(ResGroup);
        }

		bool[] originalState;
		void SetResourceDirty()
		{
			originalState = ResGroup.ActivateState;
			ResGroup.ActivateState = new bool[]{ false, false, false };
			ResourceGroupList.Instance.OnResourceGroupChange(ResGroup);
			ResGroup.ActivateState = originalState;
			ResourceGroupList.Instance.OnResourceGroupChange(ResGroup);
		}

		public void ReflreshUI()
		{
            for (int i = 1; i < ResContainor.childCount; i++) // don't destroy header
            {
                Destroy(ResContainor.GetChild(i).gameObject);
            }

            SetContent(ResGroup, manager);
        }

		void FinishEditingScale(string s)
		{
			ResGroup.Scale = float.Parse(s);
			ResGroup.SetConfig (null);
			SetResourceDirty ();
		}

		void FinishEditingXAxis(string s)
		{
			ResGroup.XAxis = float.Parse (s);
			ResGroup.SetConfig (null);
			SetResourceDirty ();
		}

		void FinishEditingYAxis(string s)
		{
			ResGroup.YAxis = float.Parse (s);
			ResGroup.SetConfig (null);
			SetResourceDirty ();
		}

		public void SetConfig()
		{
			if (ResGroup != null)
				ResGroup.SetConfig (null);
		}

		public void SelectSelf()
		{
			ResGroup.manager.OnChangeResourceSelection (this);
		}

		public void OnSelect()
		{
			BackgroundImage.color = SelectedColor;
			LayerButton.gameObject.SetActive (true);
		}

		public void OnDeselect()
		{
			LayerButton.gameObject.SetActive (false);
			BackgroundImage.color = originalColor;
		}
        
    }
}
