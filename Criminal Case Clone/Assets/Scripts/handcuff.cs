using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class handcuff : MonoBehaviour
{
    public bool reach = false;
    public Player player;
    public int index;
    public bool goingToCriminal = false;
    public Transform criminal;

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(-90, 0, -180);
    }
    private void FixedUpdate()
    {
        stackMechanic();
    }

    private void stackMechanic()
    {
        if (!goingToCriminal)
        {
            if (!reach)
            {
                var firstHandcuff = player.handCuffStack.ElementAt(1);
                var secondHandcuff = gameObject.transform;

                secondHandcuff.position = new Vector3(Mathf.Lerp(secondHandcuff.position.x, firstHandcuff.position.x, Time.deltaTime * 5f),
                Mathf.Lerp(secondHandcuff.position.y, firstHandcuff.position.y, Time.deltaTime * 5f), firstHandcuff.position.z);
                //yaklastikca yavasladigindan dolayi delay suresini azaltmak icin ifi ekledim.
                if (firstHandcuff.localPosition.y <= secondHandcuff.localPosition.y + 0.15f)
                {
                    reach = true;
                }
            }
            else
            {
                var firstHandcuff = player.handCuffStack.ElementAt(index - 1);
                var secondHandcuff = transform;
                secondHandcuff.position = new Vector3(Mathf.Lerp(secondHandcuff.position.x, firstHandcuff.position.x, Time.deltaTime * 5f),
                Mathf.Lerp(secondHandcuff.position.y, firstHandcuff.position.y + 0.2f, Time.deltaTime * 5f), firstHandcuff.position.z);
                if (firstHandcuff.localPosition == secondHandcuff.localPosition)
                {
                    this.enabled = false;
                }
            }
        }
        else
        {

            if (!reach)
            {
                var firstHandcuff = player.handCuffStack.ElementAt(1);
                var secondHandcuff = gameObject.transform;

                secondHandcuff.position = new Vector3(Mathf.Lerp(secondHandcuff.position.x, firstHandcuff.position.x, Time.deltaTime * 5f),
                Mathf.Lerp(secondHandcuff.position.y, firstHandcuff.position.y, Time.deltaTime * 5f), firstHandcuff.position.z);
                //yaklastikca yavasladigindan dolayi delay suresini azaltmak icin ifi ekledim.
                if (firstHandcuff.localPosition.y <= secondHandcuff.localPosition.y + 0.1f)
                {
                    reach = true;
                }
            }
            else
            {
                var firstHandcuff = criminal;
                var secondHandcuff = transform;
                transform.parent = criminal;
                secondHandcuff.position = new Vector3(Mathf.Lerp(secondHandcuff.position.x, firstHandcuff.position.x, Time.deltaTime * 5f),
                Mathf.Lerp(secondHandcuff.position.y, firstHandcuff.position.y + 0.2f, Time.deltaTime * 25f), firstHandcuff.position.z);
                float distance = Vector3.Distance(criminal.transform.position, transform.position);
                if (distance < 0.25f)
                {
                    reach = false;
                    goingToCriminal = false;
                    transform.localPosition = new Vector3(0, 0, 0.5f);
                    transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    this.enabled = false;
                }
            }
        }
    }
}
