#if SET_ADDRESSABLE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SongLib
{
    public class GameAddressableSpriteAtlas : GameAddressable<SpriteAtlas>
    {
        protected Dictionary<string, Sprite> spriteResources = new Dictionary<string, Sprite>();

        public override async Awaitable Init(string label = "PreLoad")
        {
            await base.Init(label);

            foreach (var spriteAtlas in GetAllResource().Values)
            {
                Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(sprites);

                foreach (var sprite in sprites)
                {
                    // MEMO: ”(Cl“은 스프라이트가 압축될 때 스프라이트 이름에 추가되는 문자열
                    int length = sprite.name.IndexOf("(Cl", StringComparison.Ordinal);
                    if (length > 0)
                    {
                        spriteResources[sprite.name.Substring(0, length)] = sprite;
                    }
                }
            }
        }
        
        public Sprite GetSprite(string fileName)
        {
            return spriteResources.GetValueOrDefault(fileName);
        }
    }
}
#endif