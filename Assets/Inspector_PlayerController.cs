using marianateixeira.PlayerController;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(PlayerController))]
public class Inspector_PlayerController : Editor
{
    PlayerController playerController;

    private void OnEnable()
    {
        playerController = (PlayerController)target;
    }
    private void OnDisable()
    {
        playerController = null;
    }

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        
        PropertyField move = new PropertyField();
        move.bindingPath = "Move";

        PropertyField data = new PropertyField();
        data.bindingPath = "Data";

        Button recalculateButton = new Button() { text = "Recalculate Physics" };
        recalculateButton.clicked += playerController.RecalculatePhysics;

        root.Add(move);
        root.Add(data);
        root.Add(recalculateButton);
        return root;
    }
}