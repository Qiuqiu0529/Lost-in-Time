using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    MovingPlatform MovingPlatform;

    private SerializedProperty m_IsMovingAtStartProperty;
    //private SerializedProperty m_IsLadderProperty;
    private SerializedProperty m_StartMovingOnlyWhenVisibleProperty;
    private SerializedProperty m_PlatformTypeProperty;
    private SerializedProperty m_PlatformSpeedProperty;
    private SerializedProperty m_PlatformNodesProperty;
    private SerializedProperty m_PlatformWaitTimeProperty;
    private SerializedProperty m_PlatformSpeedCurveProperty;
    private SerializedProperty m_PlatformUseSpeedCurveProperty;
    private SerializedProperty m_PlatformStartDisProperty;

    private void OnEnable()
    {
        MovingPlatform = target as MovingPlatform;
       // m_IsLadderProperty= serializedObject.FindProperty(nameof(MovingPlatform.isLadder));
        m_IsMovingAtStartProperty = serializedObject.FindProperty(nameof(MovingPlatform.isMovingAtStart));
        m_StartMovingOnlyWhenVisibleProperty = serializedObject.FindProperty(nameof(MovingPlatform.startMovingOnlyWhenVisible));
        m_PlatformTypeProperty = serializedObject.FindProperty(nameof(MovingPlatform.platformType));
        m_PlatformSpeedProperty = serializedObject.FindProperty(nameof(MovingPlatform.speed));
        m_PlatformNodesProperty = serializedObject.FindProperty(nameof(MovingPlatform.localNodes));
        m_PlatformWaitTimeProperty = serializedObject.FindProperty(nameof(MovingPlatform.waitTimes));
        m_PlatformSpeedCurveProperty = serializedObject.FindProperty(nameof(MovingPlatform.speedCurve));
        m_PlatformUseSpeedCurveProperty = serializedObject.FindProperty(nameof(MovingPlatform.usespeedCurve));
        m_PlatformStartDisProperty = serializedObject.FindProperty(nameof(MovingPlatform.startDis));

    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUI.BeginChangeCheck();

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(m_IsMovingAtStartProperty);
        //EditorGUILayout.PropertyField(m_IsLadderProperty);
        if (m_IsMovingAtStartProperty.boolValue)
        {
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(m_StartMovingOnlyWhenVisibleProperty);
            EditorGUI.indentLevel -= 1;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.PropertyField(m_PlatformTypeProperty);
        EditorGUILayout.PropertyField(m_PlatformSpeedProperty);
        EditorGUILayout.PropertyField(m_PlatformStartDisProperty);


        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("Add Node"))
        {
            Undo.RecordObject(target, "added node");


            Vector3 position = MovingPlatform.localNodes[MovingPlatform.localNodes.Length - 1] + Vector3.right;

            int index = m_PlatformNodesProperty.arraySize;
            m_PlatformNodesProperty.InsertArrayElementAtIndex(index);
            m_PlatformNodesProperty.GetArrayElementAtIndex(index).vector3Value = position;

            m_PlatformWaitTimeProperty.InsertArrayElementAtIndex(index);
            m_PlatformWaitTimeProperty.GetArrayElementAtIndex(index).floatValue = 0;

            m_PlatformSpeedCurveProperty.InsertArrayElementAtIndex(index);

            m_PlatformUseSpeedCurveProperty.InsertArrayElementAtIndex(index);

        }

        EditorGUIUtility.labelWidth = 64;
        int delete = -1;
        for (int i = 0; i < MovingPlatform.localNodes.Length; ++i)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();

            int size = 64;
            EditorGUILayout.BeginVertical(GUILayout.Width(size));
            EditorGUILayout.LabelField("Node " + i, GUILayout.Width(size));
            if (i != 0 && GUILayout.Button("Delete", GUILayout.Width(size)))
            {
                delete = i;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            if (i != 0)
            {
                EditorGUILayout.PropertyField(m_PlatformNodesProperty.GetArrayElementAtIndex(i), new GUIContent("Pos"));
                EditorGUILayout.PropertyField(m_PlatformWaitTimeProperty.GetArrayElementAtIndex(i), new GUIContent("Wait Time"));
                EditorGUILayout.PropertyField(m_PlatformSpeedCurveProperty.GetArrayElementAtIndex(i), new GUIContent("SpeedCurve"));
                EditorGUILayout.PropertyField(m_PlatformUseSpeedCurveProperty.GetArrayElementAtIndex(i), new GUIContent("useSpeedCurve"));

            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

        }
        EditorGUIUtility.labelWidth = 0;

        if (delete != -1)
        {
            m_PlatformNodesProperty.DeleteArrayElementAtIndex(delete);
            m_PlatformWaitTimeProperty.DeleteArrayElementAtIndex(delete);
            m_PlatformSpeedCurveProperty.DeleteArrayElementAtIndex(delete);
            m_PlatformUseSpeedCurveProperty.DeleteArrayElementAtIndex(delete);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {

        for (int i = 0; i < MovingPlatform.localNodes.Length; ++i)
        {
            Vector3 worldPos;
            if (Application.isPlaying)
            {
                worldPos = MovingPlatform.worldNode[i];
            }
            else
            {
                worldPos = MovingPlatform.transform.TransformPoint(MovingPlatform.localNodes[i]);
            }


            Vector3 newWorld = worldPos;
            if (i != 0)
                newWorld = Handles.PositionHandle(worldPos, Quaternion.identity);

            Handles.color = Color.red;

            if (i == 0)
            {
                if (MovingPlatform.platformType != MovingPlatform.MovingPlatformType.LOOP)
                    continue;

                if (Application.isPlaying)
                {
                    Handles.DrawDottedLine(worldPos, MovingPlatform.worldNode[MovingPlatform.worldNode.Length - 1], 10);
                }
                else
                {
                    Handles.DrawDottedLine(worldPos, MovingPlatform.transform.TransformPoint(MovingPlatform.localNodes[MovingPlatform.localNodes.Length - 1]), 10);
                }
            }
            else
            {
                if (Application.isPlaying)
                {
                    Handles.DrawDottedLine(worldPos, MovingPlatform.worldNode[i - 1], 10);
                }
                else
                {
                    Handles.DrawDottedLine(worldPos, MovingPlatform.transform.TransformPoint(MovingPlatform.localNodes[i - 1]), 10);
                }

                if (worldPos != newWorld)
                {
                    Undo.RecordObject(target, "moved point");

                    m_PlatformNodesProperty.GetArrayElementAtIndex(i).vector3Value = MovingPlatform.transform.InverseTransformPoint(newWorld);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }

}
#endif