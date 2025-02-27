using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.Core
{
    [CreateAssetMenu(menuName = "SO/ObjectList", order = 0)]
    public class ObjectListSO : ScriptableObject
    {
        public List<EditorObject> objects = new List<EditorObject>();

        [SerializeField] private string objectPath;

        [ContextMenu("Add Objects")]
        public void AddObjects()
        {
            if (!Directory.Exists(objectPath))
            {
                Debug.LogWarning("directory doesn't exist");
                return;
            }
            
            objects.Clear();
            
            string[] guides = AssetDatabase.FindAssets("t:GameObject", new[] { objectPath });

            foreach (string guid in guides)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (gameObject.TryGetComponent(out EditorObject editorObject))
                {
                    objects.Add(editorObject);
                }
            }
            
            objects = objects.OrderBy(obj => obj.ID).ToList();
        }
    }
}