#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NTD
{
    [CustomEditor(typeof(SpawnObjectHelper))]
    [CanEditMultipleObjects]
    [InitializeOnLoad]
    public class SpawnObjectEditor : Editor
    {
        SpawnObjectHelper spawnObject { get { return target as SpawnObjectHelper; } }
        public int Index;

        void OnSceneGUI()
        {
            Event e = Event.current;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (e.type == EventType.MouseDown && e.button == 0 && e.shift)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(worldRay, out hitInfo, Mathf.Infinity))
                {
                    CreateNode(spawnObject.handle, hitInfo.point, spawnObject.objectToSpawn);
                    EditorUtility.SetDirty(spawnObject.handle);
                }
            }
        }

        void CreateNode(Transform handle, Vector3 position, GameObject originalObject)
        {
            var Obj = Instantiate(originalObject);
            Obj.transform.position = position;
            Obj.transform.parent = handle.transform;
            Index++;
        }
    }
}
#endif