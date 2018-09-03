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

        public void SetContent(ResourceGroup g)
        {
			SelectionPanel = BackgroundImage.GetComponent<Button> ();
			originalColor = BackgroundImage.color;
            ResContainor.parent = ResContainor;
            XAxis.text = g.XAxis.ToString();
            YAxis.text = g.YAxis.ToString();
            Scale.text = g.Scale.ToString();
            Duration.text = g.Duration.ToString();
            ResGroup = g;
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
                objAttr.SetContent(g.ResourceRefs[i].Resources.Attrs);
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

		void FinishEditingScale(ResourceGroup group)
		{
			group.ActivateState = new bool[]{ false, false, true };
			GroupTraveller.Instance.OnTravel (group);
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
