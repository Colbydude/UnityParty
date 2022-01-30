using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityParty.Events;

namespace UnityParty.UI
{
    public class UIManager : MonoBehaviour
    {
        // =====================================================
        // Publics
        public GameObject ArrowButton;
        public GameEvent ArrowSelectedEvent;
        public float ArrowDistance = 50f;

        public TMPro.TMP_Text RemainingSpacesText;

        // =====================================================
        // Privates
        private Dictionary<GameObject, BoardSpace> m_arrowMapping = new Dictionary<GameObject, BoardSpace>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DrawPathSelectArrows(EventContext e)
        {
            // get possible spaces from EventContext
            List<BoardSpace> spaces = e.Caller.GetComponent<BoardSpace>().NextSpace;
            BoardSpace currentSpace = e.Caller.GetComponent<BoardSpace>();

            // create arrows for each possible space to move to
            foreach (BoardSpace space in spaces)
            {
                // get vector between current space and this space
                Vector3 direction = currentSpace.transform.position - space.transform.position;

                // move z position to y (canvas uses y)
                direction.y = direction.z;
                direction.z = 0;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // instantiate new arrow button from prefab
                Vector3 position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0) - (direction.normalized * ArrowDistance);
                GameObject newArrow = Instantiate(ArrowButton, position, Quaternion.identity);

                // assign arrow gameobject to be child of UI and update rotation
                newArrow.GetComponent<RectTransform>().SetParent(transform);
                newArrow.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
                newArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                newArrow.SetActive(true);

                // add to dictionary so we know which space to go to on select
                m_arrowMapping.Add(newArrow, space);

                // add onclick event listener
                newArrow.GetComponent<Button>().onClick.AddListener(delegate { OnArrowSelect(newArrow); });

                EventSystem.current.SetSelectedGameObject(newArrow);
            }
        }

        public void OnArrowSelect(GameObject arrow)
        { 
            // event parameters 
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("next_space", m_arrowMapping[arrow]);
            EventContext e = new EventContext(gameObject, parameters);

            // remove arrows
            foreach (GameObject arrowButton in m_arrowMapping.Keys)
            {
                Destroy(arrowButton);
            }

            // now fire off event
            ArrowSelectedEvent.Invoke(e);
        }
    }
}
