using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Work.HN.Code.MapMaker.Objects;

namespace Work.HN.Code.MapMaker.Core
{
    [CreateAssetMenu(menuName = "SO/ObjectList", order = 0)]
    public class ObjectListSO : ScriptableObject
    {
        public List<EditorObject> objects = new List<EditorObject>();
    }
}