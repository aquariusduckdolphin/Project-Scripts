using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatZone.Character.Player
{

    public class FootstepSoundPlayer : MonoBehaviour
    {

        [SerializeField] private FootstepSound footstepSounds;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerMovementSettings playerSettings;
        [SerializeField] private float playerRadius = 0.5f;
        [SerializeField] private AudioSource audioSource;

        private RaycastHit hit;

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            movement = GetComponent<PlayerMovement>();
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(CheckGround());
        }
        #endregion

        /**************** Footstep Sound Functions ****************/

        #region Check Ground
        private IEnumerator CheckGround()
        {
            while (true)
            {
                if (movement.grounded && rb.linearVelocity != Vector3.zero && Raycast(footstepSounds.floorLayer))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("House"))
                    {
                        if (audioSource.isPlaying) { audioSource.Stop(); }
                        audioSource.clip = footstepSounds.roofTopSound;
                        audioSource.Play();
                        yield return new WaitForSeconds(audioSource.clip.length);
                        continue;
                    }

                    if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                    {
                        yield return StartCoroutine(PlayFootstepSoundFromTerrain(terrain, hit.point));
                    }
                    else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        yield return StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
                    }
                }
                yield return null;
            }
        }
        #endregion

        #region Raycast
        private bool Raycast(LayerMask collisionMask)
        {
            bool rays = Physics.Raycast(transform.position - new Vector3(0, 0.5f * playerSettings.playerHeight + 0.5f * playerRadius, 0),
                        Vector3.down,
                        out hit,
                        1f,
                        collisionMask);
            return rays;
        }
        #endregion

        #region Play Footstep Sound Terrain
        private IEnumerator PlayFootstepSoundFromTerrain(Terrain Terrain, Vector3 HitPoint)
        {
            Vector3 terrainPosition = HitPoint - Terrain.transform.position;
            Vector3 splatMapPosition = new Vector3(
                terrainPosition.x / Terrain.terrainData.size.x,
                0,
                terrainPosition.z / Terrain.terrainData.size.z
            );

            int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);
            float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            if (!footstepSounds.blendTerrainSounds)
            {
                int primaryIndex = 0;
                for (int i = 1; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
                    {
                        primaryIndex = i;
                    }
                }

                foreach (TextureSound textureSound in footstepSounds.textureSounds)
                {
                    if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                    {
                        AudioClip clip = footstepSounds.GetClipFromTextureSound(textureSound);
                        audioSource.PlayOneShot(clip);
                        yield return new WaitForSeconds(clip.length);
                        break;
                    }
                }
            }
            else
            {
                List<AudioClip> clips = new List<AudioClip>();
                int clipIndex = 0;
                for (int i = 0; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0, 0, i] > 0)
                    {
                        foreach (TextureSound textureSound in footstepSounds.textureSounds)
                        {
                            if (textureSound.Albedo == Terrain.terrainData.terrainLayers[i].diffuseTexture)
                            {
                                AudioClip clip = footstepSounds.GetClipFromTextureSound(textureSound);
                                audioSource.PlayOneShot(clip, alphaMap[0, 0, i]);
                                clips.Add(clip);
                                clipIndex++;
                                break;
                            }
                        }
                    }
                }

                float longestClip = clips.Max(clip => clip.length);
                yield return new WaitForSeconds(longestClip);
            }
        }
        #endregion

        #region Play Footstep Sound Renderer
        private IEnumerator PlayFootstepSoundFromRenderer(Renderer Renderer)
        {
            foreach (TextureSound textureSound in footstepSounds.textureSounds)
            {
                if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
                {
                    AudioClip clip = footstepSounds.GetClipFromTextureSound(textureSound);
                    audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
        }
        #endregion

    }

}