using System.Collections;

using UnityEngine;

namespace BarthaSzabolcs.Tutorial_SpriteFlash
{
    public class SimpleFlash : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [Tooltip("Material to switch to during the flash.")]
        [SerializeField] private Material flashMaterial;
        //-------[SerializeField] private Material blobflashMaterial;

        [Tooltip("Duration of the flash.")]

        #endregion
        #region Private Fields

        // The SpriteRenderer that should flash.
        private SpriteRenderer spriteRenderer;
        //-------[SerializeField] SpriteShapeRenderer blobRenderer;

        // The material that was in use, when the script started.
        private Material originalMaterial;
        private Material originalBlobMaterial;

        // The currently running coroutine.
        private Coroutine flashRoutine;

        public Color whiteColor;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        void Start()
        {
            // Get the SpriteRenderer to be used,
            // alternatively you could set it from the inspector.
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            whiteColor = Color.white;
            // Get the material that the SpriteRenderer uses, 
            // so we can switch back to it after the flash ended.
            originalMaterial = spriteRenderer.material;
            //-------originalBlobMaterial = blobRenderer.material;
        }

        #endregion

        public void Flash(float duration = 1f)
        {
            // If the flashRoutine is not null, then it is currently running.
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine(duration));
        }

        private IEnumerator FlashRoutine(float duration)
        {
            // Swap to the flashMaterial.

            //if this gameobject has an script attached called 'colorController', deactivate it
            colorController colorController = GetComponent<colorController>();
            Color oldColor = spriteRenderer.color ;
            if (colorController != null)
            {
                colorController.enabled = false;
            }

            spriteRenderer.material = flashMaterial;
            spriteRenderer.color= whiteColor;

            //------- blobRenderer.material = blobflashMaterial;

            // Pause the execution of this function for "duration" seconds.
            yield return new WaitForSeconds(duration);

            // After the pause, swap back to the original material.
            if (colorController != null)
            {
                colorController.enabled = true;
            }
            spriteRenderer.color = oldColor;
            spriteRenderer.material = originalMaterial;

            //------- blobRenderer.material = originalBlobMaterial;

            // Set the routine to null, signaling that it's finished.
            flashRoutine = null;
        }

        #endregion
    }
}