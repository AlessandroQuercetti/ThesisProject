using UnityEngine;

public class InteractableHighlighter : MonoBehaviour
{
    public float rayDistance = 10f;

    private Outline currentOutline;

    void Update()
    {
        Ray ray = new Ray(
            transform.position,
            transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Outline outline =
                hit.collider.GetComponent<Outline>();

            if (outline != null)
            {
                if (currentOutline != outline)
                {
                    ClearCurrent();

                    currentOutline = outline;

                    currentOutline.enabled = true;
                }
            }
            else
            {
                ClearCurrent();
            }
        }
        else
        {
            ClearCurrent();
        }
    }

    void ClearCurrent()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}