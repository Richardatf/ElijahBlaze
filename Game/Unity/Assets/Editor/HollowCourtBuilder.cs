using System.Collections.Generic;
using ElijahBlaze.CameraSystem;
using ElijahBlaze.Combat;
using ElijahBlaze.Core;
using ElijahBlaze.Enemies;
using ElijahBlaze.Player;
using ElijahBlaze.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElijahBlaze.EditorTools
{
    public static class HollowCourtBuilder
    {
        private const string SceneFolder = "Assets/Scenes";
        private const string ScenePath = SceneFolder + "/HollowCourt.unity";

        [MenuItem("Elijah Blaze/Build Hollow Court Prototype")]
        public static void Build()
        {
            if (!AssetDatabase.IsValidFolder(SceneFolder))
                AssetDatabase.CreateFolder("Assets", "Scenes");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "HollowCourt";

            Material stone = CreateMaterial("Hollow Basalt", new Color(0.075f, 0.095f, 0.12f));
            Material architecture = CreateMaterial("Living Architecture", new Color(0.16f, 0.27f, 0.34f));
            Material shard = CreateMaterial("Shardling", new Color(0.34f, 0.12f, 0.075f));
            Material playerMaterial = CreateMaterial("Elijah Blue", new Color(0.08f, 0.3f, 0.68f));
            Material gateMaterial = CreateMaterial("Resonance Gate", new Color(0.62f, 0.4f, 0.12f), true);
            Material inlayMaterial = CreateMaterial("Resonance Inlay", new Color(0.22f, 0.48f, 0.54f), true);

            CreateLighting(gateMaterial);
            CreateCourt(stone, architecture, inlayMaterial);

            GameObject player = CreatePlayer(playerMaterial);
            CreateCamera(player.transform);

            var enemyHealth = new List<Health>();
            enemyHealth.Add(CreateShardling(new Vector3(-5f, 1f, 2f), player.transform, shard, "Shardling_A"));
            enemyHealth.Add(CreateShardling(new Vector3(0f, 1f, 5f), player.transform, shard, "Shardling_B"));
            enemyHealth.Add(CreateShardling(new Vector3(5f, 1f, 2f), player.transform, shard, "Shardling_C"));

            GameObject encounterObject = new GameObject("HollowCourt_Encounter");
            HollowCourtEncounter encounter = encounterObject.AddComponent<HollowCourtEncounter>();
            encounter.Configure(enemyHealth);
            EditorUtility.SetDirty(encounter);

            CreateGate(encounter, gateMaterial);
            CreateObjectiveMarker();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeGameObject = player;
            Debug.Log("Hollow Court prototype built at " + ScenePath);
        }

        private static void CreateLighting(Material gateMaterial)
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(0.78f, 0.84f, 0.92f);
            light.intensity = 1.25f;
            lightObject.transform.rotation = Quaternion.Euler(42f, -28f, 0f);

            RenderSettings.ambientLight = new Color(0.22f, 0.25f, 0.3f);
            CreatePointLight("Gate_Warmth", new Vector3(0f, 4.6f, 13f), new Color(1f, 0.67f, 0.28f), 2.2f, 12f);
            CreatePointLight("Court_West_Light", new Vector3(-10f, 4f, 1f), new Color(0.4f, 0.66f, 0.78f), 1.1f, 13f);
            CreatePointLight("Court_East_Light", new Vector3(10f, 4f, 8f), new Color(0.4f, 0.66f, 0.78f), 1.1f, 13f);
        }

        private static void CreatePointLight(string name, Vector3 position, Color color, float intensity, float range)
        {
            GameObject lightObject = new GameObject(name);
            lightObject.transform.position = position;
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
        }

        private static void CreateCourt(Material stone, Material architecture, Material inlay)
        {
            // The court is a long processional hall with three readable zones:
            // entry, open combat floor, and the sealed resonance threshold.
            CreateBlock("Court_Floor", new Vector3(0f, -0.5f, 2f), new Vector3(26f, 1f, 34f), stone);
            CreateBlock("West_Wall", new Vector3(-13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("East_Wall", new Vector3(13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("Back_Wall", new Vector3(0f, 3f, 18.5f), new Vector3(26f, 7f, 1f), stone);

            // Long floor joints give the space scale and lead the eye toward the gate.
            for (int i = -2; i <= 2; i++)
            {
                float x = i * 4f;
                CreateInlay("Floor_Long_Joint_" + (i + 3), new Vector3(x, 0.012f, 2f), new Vector3(0.045f, 0.025f, 32f), inlay);
            }
            float[] crossJoints = { -10f, -5f, 0f, 10f };
            for (int i = 0; i < crossJoints.Length; i++)
                CreateInlay("Floor_Cross_Joint_" + (i + 1), new Vector3(0f, 0.014f, crossJoints[i]), new Vector3(24f, 0.025f, 0.045f), inlay);

            // Repeating ribs make the side walls read as carved architecture instead of a box.
            float[] ribZ = { -10f, -3f, 4f, 11f, 16f };
            for (int i = 0; i < ribZ.Length; i++)
            {
                float height = (i % 2 == 0) ? 6.8f : 5.8f;
                CreateBlock("West_Wall_Rib_" + (i + 1), new Vector3(-12.35f, height * 0.5f, ribZ[i]), new Vector3(0.9f, height, 1.5f), architecture);
                CreateBlock("East_Wall_Rib_" + (i + 1), new Vector3(12.35f, height * 0.5f, ribZ[i]), new Vector3(0.9f, height, 1.5f), architecture);
                CreateBlock("West_Rib_Cap_" + (i + 1), new Vector3(-12.35f, height + 0.18f, ribZ[i]), new Vector3(1.35f, 0.36f, 1.95f), architecture);
                CreateBlock("East_Rib_Cap_" + (i + 1), new Vector3(12.35f, height + 0.18f, ribZ[i]), new Vector3(1.35f, 0.36f, 1.95f), architecture);
            }

            // Entry arch marks the way in and frames the player on a fresh run.
            CreateBlock("Entry_Pier_West", new Vector3(-5.2f, 2.35f, -13.2f), new Vector3(1.1f, 4.7f, 1.5f), architecture);
            CreateBlock("Entry_Pier_East", new Vector3(5.2f, 2.35f, -13.2f), new Vector3(1.1f, 4.7f, 1.5f), architecture);
            CreateBlock("Entry_Lintel", new Vector3(0f, 4.95f, -13.2f), new Vector3(11.5f, 0.9f, 1.5f), architecture);
            CreateInlay("Entry_Crown_Inlay", new Vector3(0f, 5.43f, -13.2f), new Vector3(2.8f, 0.06f, 0.08f), inlay);

            // The encounter has a broad clear center and two looping flank lanes.
            // These offset monoliths provide cover without creating dead ends.
            Vector3[] monolithPositions =
            {
                new Vector3(-7.4f, 1.2f, -2f),
                new Vector3(7.4f, 1.2f, 1f),
                new Vector3(-7.4f, 1.2f, 6f),
                new Vector3(7.4f, 1.2f, 9f)
            };
            for (int i = 0; i < monolithPositions.Length; i++)
            {
                Vector3 position = monolithPositions[i];
                CreateBlock("Court_Monolith_" + (i + 1), position, new Vector3(1.5f, 2.4f, 1.5f), architecture);
                CreateBlock("Court_Monolith_Cap_" + (i + 1), position + new Vector3(0f, 1.28f, 0f), new Vector3(1.9f, 0.22f, 1.9f), inlay);
                CreateInlay("Court_Monolith_Face_" + (i + 1), position + new Vector3(0f, 0.15f, -0.76f), new Vector3(0.16f, 1.45f, 0.045f), inlay);
            }

            // A large inlaid encounter seal occupies the open center without blocking movement.
            Vector3 sealCenter = new Vector3(0f, 0.025f, 4.5f);
            CreateRing("Resonance_Ring_Outer", sealCenter, 6.4f, 0.11f, inlay);
            CreateRing("Resonance_Ring_Middle", sealCenter, 4.7f, 0.075f, inlay);
            CreateRing("Resonance_Ring_Inner", sealCenter, 2.8f, 0.06f, inlay);
            for (int i = 0; i < 8; i++)
            {
                float angle = Mathf.PI * 2f * i / 8f;
                Vector3 spoke = sealCenter + new Vector3(Mathf.Cos(angle) * 3.75f, 0f, Mathf.Sin(angle) * 3.75f);
                GameObject mark = CreateInlay("Seal_Radial_Mark_" + (i + 1), spoke, new Vector3(0.11f, 0.035f, 1.0f), inlay);
                mark.transform.rotation = Quaternion.Euler(0f, -angle * Mathf.Rad2Deg, 0f);
            }

            CreateInlay("Approach_Line_Left", new Vector3(-1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);
            CreateInlay("Approach_Line_Right", new Vector3(1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);

            // Three shallow threshold courses lead toward the gate; each remains traversable.
            for (int i = 0; i < 3; i++)
            {
                float stepZ = 10.7f + i * 1.05f;
                float stepHeight = 0.12f + i * 0.08f;
                CreateBlock("Gate_Approach_Course_" + (i + 1), new Vector3(0f, stepHeight * 0.5f, stepZ), new Vector3(8.4f, stepHeight, 0.8f), architecture);
            }

            // Gate frame and crown establish a clear destination silhouette.
            CreateBlock("Threshold_Left", new Vector3(-4.7f, 2.9f, 14.7f), new Vector3(1.1f, 5.8f, 1.7f), architecture);
            CreateBlock("Threshold_Right", new Vector3(4.7f, 2.9f, 14.7f), new Vector3(1.1f, 5.8f, 1.7f), architecture);
            CreateBlock("Threshold_Lintel", new Vector3(0f, 6.0f, 14.7f), new Vector3(10.5f, 1.0f, 1.7f), architecture);
            CreateInlay("Gate_Crown_Inlay", new Vector3(0f, 6.53f, 14.68f), new Vector3(2.7f, 0.08f, 0.08f), inlay);
        }

        private static GameObject CreatePlayer(Material material)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "ElijahBlue_Player";
            player.transform.position = new Vector3(0f, 1f, -10f);
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.GetComponent<Renderer>().sharedMaterial = material;
            player.AddComponent<CharacterController>();
            player.AddComponent<Health>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerAttack>();
            player.AddComponent<PlayerDeathController>();
            return player;
        }

        private static void CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 62f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.position = target.position + new Vector3(0f, 4.5f, -7f);
            CameraFollow follow = cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
            EditorUtility.SetDirty(follow);
        }

        private static Health CreateShardling(Vector3 position, Transform target, Material material, string name)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.transform.position = position;
            enemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            enemy.GetComponent<Renderer>().sharedMaterial = material;

            Health health = enemy.AddComponent<Health>();
            EnemyChase chase = enemy.AddComponent<EnemyChase>();
            EnemyAttack attack = enemy.AddComponent<EnemyAttack>();
            enemy.AddComponent<DeathFeedback>();
            chase.SetTarget(target);
            attack.SetTarget(target);
            EditorUtility.SetDirty(chase);
            EditorUtility.SetDirty(attack);
            return health;
        }

        private static void CreateGate(HollowCourtEncounter encounter, Material material)
        {
            GameObject gateRoot = new GameObject("Resonance_Gate");
            gateRoot.transform.position = new Vector3(0f, 0f, 15f);

            GameObject gateBody = CreateBlock("Gate_Body", new Vector3(0f, 2.5f, 15f), new Vector3(7f, 5f, 0.8f), material);
            gateBody.transform.SetParent(gateRoot.transform, true);
            CreateInlay("Gate_Sigil_Vertical", new Vector3(0f, 5.1f, 14.48f), new Vector3(0.12f, 1.6f, 0.06f), material);

            ResonanceGate gate = gateRoot.AddComponent<ResonanceGate>();
            gate.Configure(encounter, gateBody.transform, new Vector3(0f, 6f, 0f));
            EditorUtility.SetDirty(gate);
        }

        private static void CreateObjectiveMarker()
        {
            GameObject marker = new GameObject("Objective_ReachTheGate");
            marker.transform.position = new Vector3(0f, 0f, 13f);
        }

        private static GameObject CreateBlock(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.position = position;
            block.transform.localScale = scale;
            block.GetComponent<Renderer>().sharedMaterial = material;
            return block;
        }

        private static void CreateInlay(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject inlay = GameObject.CreatePrimitive(PrimitiveType.Cube);
            inlay.name = name;
            inlay.transform.position = position;
            inlay.transform.localScale = scale;
            Object.DestroyImmediate(inlay.GetComponent<Collider>());
            inlay.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static void CreateRing(string name, Vector3 center, float radius, float width, Material material)
        {
            GameObject ring = new GameObject(name);
            ring.transform.position = center;
            LineRenderer line = ring.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.loop = true;
            line.positionCount = 64;
            line.startWidth = width;
            line.endWidth = width;
            line.sharedMaterial = material;
            line.startColor = material.color;
            line.endColor = material.color;
            for (int i = 0; i < line.positionCount; i++)
            {
                float angle = Mathf.PI * 2f * i / line.positionCount;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius));
            }
        }

        private static Material CreateMaterial(string name, Color color, bool emissive = false)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Lit");

            Material material = new Material(shader) { name = name, color = color };
            material.SetFloat("_Metallic", 0.18f);
            material.SetFloat("_Glossiness", 0.38f);
            if (emissive)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 0.3f);
            }
            return material;
        }
    }
}
