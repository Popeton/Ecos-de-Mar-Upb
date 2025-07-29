using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GlowEffectPulse : MonoBehaviour
{
    private Renderer rend;
    private Material glowMaterialInstance;

    [Header("Glow Settings")]
    [SerializeField] private Material glowMaterial;
    [SerializeField] private Color baseColor = Color.cyan;
    [SerializeField] private float minIntensity = 1.5f;
    [SerializeField] private float maxIntensity = 4.0f;
    [SerializeField] private float fresnelValue = 3.0f;
    [SerializeField] private float speed = 2.0f;

    void OnEnable()
    {
        InitializeGlowMaterial();
    }

    void InitializeGlowMaterial()
    {
        rend = GetComponent<Renderer>();
        if (rend == null) return;

        // Cargar material base
        Material baseGlow = glowMaterial;
        if (baseGlow == null) {
            Debug.LogError("Glow material no encontrado en Resources/Materials/");
            return;
        }

        // Crear instancia única si no existe
        if (glowMaterialInstance == null) {
            glowMaterialInstance = Instantiate(baseGlow);
        }

        // Buscar posición disponible
        List<Material> materials = rend.sharedMaterials.ToList();
        int targetIndex = -1;

        // Buscar desde índice 1 hacia adelante
        for (int i = 1; i < materials.Count + 1; i++) {
            if (i >= materials.Count || materials[i] != glowMaterialInstance) {
                targetIndex = i;
                break;
            }
        }

        // Asegurar tamaño de la lista
        while (materials.Count <= targetIndex) {
            materials.Add(null);
        }

        // Asignar material glow
        materials[targetIndex] = glowMaterialInstance;
        rend.materials = materials.ToArray();
    }

    void Update()
    {
        if (glowMaterialInstance != null) {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * speed, 1));
            glowMaterialInstance.SetColor("_Color", baseColor * intensity);
            glowMaterialInstance.SetFloat("_FresnelPower", fresnelValue);
        }
    }

    void OnDisable()
    {
        if (rend != null && glowMaterialInstance != null) {
            // Remover material glow
            List<Material> materials = rend.sharedMaterials.ToList();
            materials.RemoveAll(m => m == glowMaterialInstance);
            rend.materials = materials.ToArray();

            // Destruir instancia
            Destroy(glowMaterialInstance);
            glowMaterialInstance = null;
        }
    }
}
