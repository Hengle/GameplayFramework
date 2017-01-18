using UnityEngine;

public class TestButton : MonoBehaviour
{

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    public string ButtonName = "Test";

    public Rect Rect = new Rect(10,10,40,25);

    public UnityEngine.UI.Button.ButtonClickedEvent Event;

    private void OnGUI()
    {
        if (GUI.Button(Rect, ButtonName))
        {
            if (Event != null)
            {
                Event.Invoke();
            }
        }
    }

#endif

}