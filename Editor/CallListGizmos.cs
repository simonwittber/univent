using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DifferentMethods.Univents
{
    public static class CallListGizmos
    {


        [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
        static void DrawGizmos(UniventComponent uevh, GizmoType gizmoType)
        {
            Gizmos.DrawIcon(uevh.transform.position, "Univent/univenticon.png", false);
            DrawLines(uevh, 0.5f);
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active | GizmoType.Pickable)]
        static void DrawGizmosSelected(UniventComponent uevh, GizmoType gizmoType)
        {
            Gizmos.DrawIcon(uevh.transform.position, "Univent/univenticon.png", false);
            DrawLines(uevh, 1f);
        }

        private static void DrawLines(UniventComponent uevh, float power)
        {
            Vector3 position = uevh.transform.position;
            DrawLinesOnly(uevh, power);
            if (Vector3.Distance(position, Camera.current.transform.position) < 7f && power >= 1)
                DrawLabels(uevh, power);
        }

        private static void DrawLabels(UniventComponent uevh, float power)
        {
            var labels = new Dictionary<GameObject, List<string>>();
            foreach (var fi in uevh.GetType().GetFields())
            {
                if (fi.FieldType == typeof(ActionList))
                {
                    var uev = (ActionList)fi.GetValue(uevh);
                    if (uev == null) continue;
                    foreach (var mc in uev.GetCalls())
                    {
                        if (mc.gameObject == uevh.gameObject) continue;
                        if (mc.gameObject == null) continue;
                        List<string> summary;
                        if (!labels.TryGetValue(mc.gameObject, out summary))
                        {
                            summary = new List<string>();
                            labels[mc.gameObject] = summary;
                        }
                        summary.Add($"<color=yellow>{fi.Name}</color> : <color=white>{mc.NiceName()}</color>");
                    }
                }
                if (fi.FieldType == typeof(PredicateList))
                {
                    var uev = (PredicateList)fi.GetValue(uevh);
                    if (uev == null) continue;
                    foreach (var mc in uev.GetCalls())
                    {
                        if (mc.gameObject == uevh.gameObject) continue;
                        if (mc.gameObject == null) continue;
                        List<string> summary;
                        if (!labels.TryGetValue(mc.gameObject, out summary))
                        {
                            summary = new List<string>();
                            labels[mc.gameObject] = summary;
                        }
                        summary.Add($"<color=yellow>{fi.Name}</color> : <color=white>{mc.NiceName()}</color>");
                    }
                }
                GUIStyle style = new GUIStyle("box");
                style.fixedHeight = 0;
                style.alignment = TextAnchor.UpperLeft;
                style.padding = new RectOffset(5, 0, 5, 0);
                style.richText = true;
                foreach (var kv in labels)
                {
                    var distance = (kv.Key.gameObject.transform.position - uevh.transform.position).magnitude * 0.25f;
                    if (distance > 3) distance = 3;
                    var pos = Vector3.MoveTowards(uevh.transform.position, kv.Key.gameObject.transform.position, distance);
                    Handles.Label(pos, string.Join("\r\n", kv.Value), style);
                }

            }
        }

        private static void DrawLinesOnly(UniventComponent uevh, float power)
        {
            var drawn = new HashSet<GameObject>();
            foreach (var fi in uevh.GetType().GetFields())
            {
                if (fi.FieldType == typeof(ActionList))
                {
                    var c = Color.yellow;
                    c.a = power;
                    Handles.color = c;
                    var uev = (ActionList)fi.GetValue(uevh);
                    if (uev == null) continue;
                    foreach (var mc in uev.GetCalls())
                        if (mc.gameObject != null && !drawn.Contains(mc.gameObject))
                        {
                            var dir = -(mc.gameObject.transform.position - uevh.transform.position).normalized;
                            Handles.DrawDottedLine(uevh.transform.position, mc.gameObject.transform.position, 3);
                            var offset = HandleUtility.GetHandleSize(mc.gameObject.transform.position) / 2;
                            Gizmos.DrawIcon(mc.gameObject.transform.position + dir * offset, "Univent/univenteffectedicon.png", false);
                            drawn.Add(mc.gameObject);
                        }
                }
                if (fi.FieldType == typeof(PredicateList))
                {
                    var c = Color.green;
                    c.a = power;
                    Handles.color = c;
                    var uev = (PredicateList)fi.GetValue(uevh);
                    if (uev == null) continue;
                    foreach (var mc in uev.GetCalls())
                        if (mc.gameObject != null && !drawn.Contains(mc.gameObject))
                        {
                            var dir = -(mc.gameObject.transform.position - uevh.transform.position).normalized;
                            Handles.DrawDottedLine(uevh.transform.position, mc.gameObject.transform.position, 3);
                            var offset = HandleUtility.GetHandleSize(mc.gameObject.transform.position) / 2;
                            Gizmos.DrawIcon(mc.gameObject.transform.position + dir * offset, "Univent/univenteffectedicon.png", false);
                            drawn.Add(mc.gameObject);
                        }
                }
            }
        }
    }

}
