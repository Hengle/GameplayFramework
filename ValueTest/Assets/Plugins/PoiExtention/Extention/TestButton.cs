using UnityEngine;

public class TestButton : MonoBehaviour
{

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    public Rect Rect = new Rect(10,10,40,25);

    public UnityEngine.UI.Button.ButtonClickedEvent Event;

    private void OnGUI()
    {
        if (GUI.Button(Rect,"Test"))
        {
            if (Event != null)
            {
                Event.Invoke();
            }
        }
    }

#endif

}