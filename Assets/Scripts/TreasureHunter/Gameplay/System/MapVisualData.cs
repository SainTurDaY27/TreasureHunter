using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreasureHunter.Gameplay.Map;
using UnityEngine;

namespace TreasureHunter.Gameplay.System
{
    [CreateAssetMenu(fileName = "MapVisualData", menuName = "TreasureHunter/MapVisualData")]
    public class MapVisualData : ScriptableObject
    {
        [SerializeField]
        private MapImages[] _mapImages;

        [Serializable]
        protected class MapImages
        {
            [SerializeField]
            private MapAreaKey _key;

            [SerializeField]
            private Sprite _imageSprite;

            public MapAreaKey Key => _key;
            public Sprite ImageSprite => _imageSprite;
        }

        public Sprite GetMapImage(MapAreaKey mapAreaKey)
        {
            var mapImage = _mapImages.FirstOrDefault(map => map.Key == mapAreaKey);
            if (mapImage == null)
            {
                Debug.LogError($"Map image for {mapAreaKey} not found");
                return null;
            }
            return mapImage.ImageSprite;
        }
    }
}