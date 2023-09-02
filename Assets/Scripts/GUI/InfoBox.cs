using UnityEngine;

public class InfoBox : MonoBehaviour
{

    public Info info;

    public InfoPanel panel3;
    public InfoPanel panel2;
    public InfoPanel panel1;
    private InfoPanel targetPanel = null;

    // Start is called before the first frame update
    void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (info.footerText != "")
        {
            targetPanel = panel3;
            targetPanel.footerText.text = info.footerText;
            targetPanel.titleText.text = info.titleText;
            targetPanel.bodyText.text = info.bodyText;
        }
        else if (info.titleText != "")
        {
            targetPanel = panel2;
            targetPanel.titleText.text = info.titleText;
            targetPanel.bodyText.text = info.bodyText;
        }
        else
        {
            targetPanel = panel1;
            targetPanel.bodyText.text = info.bodyText;
        }

        if (targetPanel != null) targetPanel.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
