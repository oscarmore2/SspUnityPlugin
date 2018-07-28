using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Resource
{
    public class ResourceGroupItem : MonoBehaviour
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

        public Transform ResContainor;

        public void SetContent(ResourceGroup g)
        {
            ResContainor.parent = ResContainor;
            XAxis.text = g.XAxis.ToString();
            YAxis.text = g.YAxis.ToString();
            Scale.text = g.Scale.ToString();
            Duration.text = g.Duration.ToString();

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
            }

            PVWToggle.isOn = g.ActivateState[0];
            PGMToggle.isOn = g.ActivateState[1];
            BinDiagToggle.isOn = g.ActivateState[2];

            groups.CalculateLayoutInputVertical();

            var groupss = GetComponentsInChildren<VerticalLayoutGroup>();
            foreach (var gr in groupss)
            {
                gr.CalculateLayoutInputVertical();
            }
        }
        
    }
}
