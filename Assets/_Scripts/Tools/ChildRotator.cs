using UnityEngine;

public class ChildRotator : MonoBehaviour
{
    public void RotateAllChildren()
    {
        RotateToObject[] childScripts = GetComponentsInChildren<RotateToObject>();

        foreach (RotateToObject childScript in childScripts)
        {
            childScript.RotateAndCheck();
        }
    }
}