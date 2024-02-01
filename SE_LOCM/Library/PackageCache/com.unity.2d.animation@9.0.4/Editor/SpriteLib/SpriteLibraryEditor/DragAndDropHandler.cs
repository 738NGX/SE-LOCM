using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UnityEditor.U2D.Animation.SpriteLibraryEditor
{
    internal enum SpriteSourceType
    {
        Sprite,
        Psb
    }

    internal struct DragAndDropData
    {
        public SpriteSourceType spriteSourceType;
        public string name;
        public List<Sprite> sprites;
    }

    internal static class DragAndDropHandler
    {
        public const string overlayElementName = "DragAndDropOverlay";
        const string k_DragOverAddClassName = SpriteLibraryEditorWindow.editorWindowClassName + "__drag-over-add";
        static readonly List<string> k_SupportedPsdExtensions = new() { ".psd", ".psb" };

        class DragDataReceiverTracker
        {
            public VisualElement activeElement { get; private set; }
            public bool Contains(VisualElement e) => receivingElements.Contains(e);
            List<VisualElement> receivingElements { get; } = new();

            public void AddNewElement(VisualElement receivingElement, DragAndDropVisualMode visualMode)
            {
                var lastReceivingElement = receivingElements.Count == 0 ? null : receivingElements[^1];
                lastReceivingElement?.RemoveFromClassList(k_DragOverAddClassName);

                receivingElements.Add(receivingElement);

                activeElement = receivingElement;

                if (visualMode == DragAndDropVisualMode.Copy)
                    receivingElement.AddToClassList(k_DragOverAddClassName);
            }

            public bool RemoveElement(VisualElement receivingElement)
            {
                receivingElements.Remove(receivingElement);

                var lastReceivingElement = receivingElements.Count == 0 ? null : receivingElements[^1];
                if (receivingElement.ClassListContains(k_DragOverAddClassName))
                {
                    receivingElement.RemoveFromClassList(k_DragOverAddClassName);
                    lastReceivingElement?.AddToClassList(k_DragOverAddClassName);
                }

                activeElement = lastReceivingElement;

                return receivingElements.Count == 0;
            }

            public void Clear()
            {
                foreach (var receivingElement in receivingElements)
                {
                    if (receivingElement.ClassListContains(k_DragOverAddClassName))
                        receivingElement.RemoveFromClassList(k_DragOverAddClassName);
                }

                receivingElements.Clear();
            }
        }

        static DragAndDropVisualMode DecideVisualMode()
        {
            foreach (var objectReference in DragAndDrop.objectReferences)
            {
                if (objectReference is Sprite or Texture2D)
                    return DragAndDropVisualMode.Copy;
            }

            foreach (var path in DragAndDrop.paths)
            {
                var ext = Path.GetExtension(path).ToLower();
                if (k_SupportedPsdExtensions.Contains(ext))
                    return DragAndDropVisualMode.Copy;
            }

            return DragAndDropVisualMode.Rejected;
        }

        static List<DragAndDropData> RetrieveDraggedSprites(Object[] objectReferences)
        {
            var data = new List<DragAndDropData>();
            var unassociatedSprites = new List<Sprite>();
            foreach (var objectReference in objectReferences)
            {
                switch (objectReference)
                {
                    case Sprite sprite:
                        unassociatedSprites.Add(sprite);
                        break;
                    case Texture2D texture2D:
                    {
                        var texturePath = AssetDatabase.GetAssetPath(texture2D);
                        var spritesFromTexture = new List<Sprite>();
                        foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(texturePath))
                        {
                            if (obj is Sprite)
                                spritesFromTexture.Add((Sprite)obj);
                        }

                        var textureData = new DragAndDropData
                        {
                            name = Path.GetFileNameWithoutExtension(texturePath),
                            sprites = new List<Sprite>(spritesFromTexture),
                            spriteSourceType = SpriteSourceType.Sprite
                        };

                        data.Add(textureData);
                        break;
                    }
                    case GameObject gameObject:
                    {
                        var isPsdGameObjectRoot = gameObject.transform.parent != null;
                        if (isPsdGameObjectRoot)
                            continue;

                        var psdFilePath = AssetDatabase.GetAssetPath(gameObject);
                        if (string.IsNullOrEmpty(psdFilePath))
                            continue;

                        var ext = Path.GetExtension(psdFilePath);
                        if (k_SupportedPsdExtensions.Contains(ext))
                        {
                            var psdSprites = new List<Sprite>();
                            foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(psdFilePath))
                            {
                                var spriteObj = obj as Sprite;
                                if (spriteObj != null)
                                    psdSprites.Add(spriteObj);
                            }

                            var psdData = new DragAndDropData
                            {
                                name = Path.GetFileNameWithoutExtension(psdFilePath),
                                sprites = new List<Sprite>(psdSprites),
                                spriteSourceType = SpriteSourceType.Psb
                            };

                            data.Add(psdData);
                        }

                        break;
                    }
                }
            }

            if (unassociatedSprites.Count > 0)
            {
                var spritesData = new DragAndDropData
                {
                    name = unassociatedSprites[0].name,
                    sprites = unassociatedSprites,
                    spriteSourceType = SpriteSourceType.Sprite
                };
                data.Add(spritesData);
            }

            return data;
        }

        public static void SetupDragOverlay(
            VisualElement visualElement, VisualElement overlay,
            string dataKey,
            Func<bool> canStartDrag,
            Action<List<DragAndDropData>, bool> onDragPerform)
        {
            visualElement.RegisterCallback<DragEnterEvent>(evt => OnDragEnter(evt, overlay, dataKey, canStartDrag));
            visualElement.RegisterCallback<DragUpdatedEvent>(evt => OnDragUpdate(evt, overlay, dataKey));
            visualElement.RegisterCallback<DragExitedEvent>(evt => OnDragExit(evt, overlay, dataKey));
            visualElement.RegisterCallback<DragLeaveEvent>(evt => OnDragLeave(evt, overlay, dataKey));
            visualElement.RegisterCallback<DragPerformEvent>(evt => OnDragPerform(evt, overlay, dataKey, onDragPerform));
        }

        static void OnDragEnter(DragEnterEvent evt, VisualElement receivingElement, string dataKey, Func<bool> canStartDrag)
        {
            // Early out when list is reordered
            if (DragAndDrop.GetGenericData("user_data") != null)
                return;

            if (!canStartDrag())
                return;

            var visualMode = DecideVisualMode();
            var dragData = DragAndDrop.GetGenericData(dataKey) as DragDataReceiverTracker ?? new DragDataReceiverTracker();
            dragData.AddNewElement(receivingElement, visualMode);
            DragAndDrop.SetGenericData(dataKey, dragData);

            evt.StopPropagation();
        }

        static void OnDragUpdate(DragUpdatedEvent evt, VisualElement receivingElement, string dataKey)
        {
            if (DragAndDrop.GetGenericData(dataKey) is DragDataReceiverTracker tracker
                && tracker.Contains(receivingElement))
            {
                DragAndDrop.visualMode = DecideVisualMode();

                evt.StopPropagation();
            }
        }

        static void OnDragExit(DragExitedEvent evt, VisualElement receivingElement, string dataKey)
        {
            if (DragAndDrop.GetGenericData(dataKey) is DragDataReceiverTracker tracker
                && tracker.Contains(receivingElement))
            {
                if (tracker.RemoveElement(receivingElement))
                    DragAndDrop.SetGenericData(dataKey, null);

                evt.StopPropagation();
            }
        }

        static void OnDragLeave(DragLeaveEvent evt, VisualElement receivingElement, string dataKey)
        {
            if (DragAndDrop.GetGenericData(dataKey) is DragDataReceiverTracker tracker
                && tracker.Contains(receivingElement))
            {
                if (tracker.RemoveElement(receivingElement))
                    DragAndDrop.SetGenericData(dataKey, null);

                evt.StopPropagation();
            }
        }

        static void OnDragPerform(DragPerformEvent evt, VisualElement receivingElement, string dataKey, Action<List<DragAndDropData>, bool> onDragPerform)
        {
            if (DragAndDrop.GetGenericData(dataKey) is DragDataReceiverTracker tracker && tracker.activeElement == receivingElement)
            {
                tracker.Clear();
                DragAndDrop.SetGenericData(dataKey, null);

                var spritesData = RetrieveDraggedSprites(DragAndDrop.objectReferences);
                if (spritesData.Count == 0)
                    return;

                onDragPerform?.Invoke(spritesData, evt.altKey);
                evt.StopPropagation();
            }
        }
    }
}