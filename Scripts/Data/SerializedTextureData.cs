using System;
using UnityEngine;

/// <summary>
/// Unity texture serializer data namespace
/// </summary>
namespace UnityTextureSerializer.Data
{
    /// <summary>
    /// Serialized texture data class
    /// </summary>
    [Serializable]
    public class SerializedTextureData
    {
        /// <summary>
        /// Size
        /// </summary>
        [SerializeField]
        private Vector2Int size = Vector2Int.one;

        /// <summary>
        /// Texture format
        /// </summary>
        [SerializeField]
        private TextureFormat textureFormat = TextureFormat.ARGB32;

        /// <summary>
        /// Mip count
        /// </summary>
        [SerializeField]
        private int mipCount = default;

        /// <summary>
        /// Is linear
        /// </summary>
        [SerializeField]
        private bool linear = default;

        /// <summary>
        /// PNG data
        /// </summary>
        [SerializeField]
        private string pngData;

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Sprite
        /// </summary>
        private Sprite sprite;

        /// <summary>
        /// Size
        /// </summary>
        public Vector2Int Size => size;

        /// <summary>
        /// PNG data
        /// </summary>
        public string PNGData
        {
            get
            {
                if (pngData == null)
                {
                    texture = new Texture2D(size.x, size.y);
                    pngData = Convert.ToBase64String(texture.EncodeToPNG());
                }
                else if (pngData.Length <= 0)
                {
                    texture = new Texture2D(size.x, size.y);
                    pngData = Convert.ToBase64String(texture.EncodeToPNG());
                }
                return pngData;
            }
        }

        /// <summary>
        /// Texture
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    try
                    {
                        byte[] texture_data = Convert.FromBase64String(PNGData);
                        if (texture_data != null)
                        {
                            texture = new Texture2D(size.x, size.y, textureFormat, mipCount, linear);
                            texture.LoadImage(texture_data);
                            texture.Apply();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    if (texture == null)
                    {
                        texture = new Texture2D(size.x, size.y);
                    }
                }
                return texture;
            }
            set
            {
                if (value != null)
                {
                    sprite = null;
                    linear = (value.filterMode != FilterMode.Point);
                    if (value.isReadable)
                    {
                        texture = value;
                    }
                    else
                    {
                        texture = new Texture2D(value.width, value.height, value.format, value.mipmapCount, linear);
                        Graphics.CopyTexture(value, texture);
                    }
                    size = new Vector2Int(texture.width, texture.height);
                    textureFormat = texture.format;
                    mipCount = texture.mipmapCount;
                    pngData = Convert.ToBase64String(texture.EncodeToPNG());
                }
            }
        }

        /// <summary>
        /// Sprite
        /// </summary>
        public Sprite Sprite
        {
            get
            {
                if (sprite == null)
                {
                    sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, size.x, size.y), new Vector2(size.x * 0.5f, size.y * 0.5f));
                }
                return sprite;
            }
            set
            {
                if (value != null)
                {
                    Texture = value.texture;
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SerializedTextureData()
        {
            // ...
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="serializedTexture">Serialized texture</param>
        public SerializedTextureData(SerializedTextureData serializedTexture)
        {
            if (serializedTexture != null)
            {
                size = serializedTexture.Size;
                textureFormat = serializedTexture.textureFormat;
                mipCount = serializedTexture.mipCount;
                linear = serializedTexture.linear;
                pngData = serializedTexture.PNGData;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">Size</param>
        /// <param name="textureFormat">Texture format</param>
        /// <param name="mipCount">Mip count</param>
        /// <param name="linear">Is linear</param>
        /// <param name="pngData">PNG data</param>
        public SerializedTextureData(Vector2Int size, TextureFormat textureFormat, int mipCount, bool linear, string pngData)
        {
            this.size = size;
            this.textureFormat = textureFormat;
            this.mipCount = mipCount;
            this.linear = linear;
            this.pngData = pngData;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="texture">Texture</param>
        public SerializedTextureData(Texture2D texture)
        {
            Texture = texture;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sprite">Sprite</param>
        public SerializedTextureData(Sprite sprite)
        {
            Sprite = sprite;
        }
    }
}
