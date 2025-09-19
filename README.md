👁️‍🗨️ Hide & Be Seek’d — Indie Horror (Unity)












Short, tense horror prototype built solo during year 3: jumpscares, spatialized audio, AI that reacts to noise and sight, simple puzzles, and first-person immersion (head-bob, footsteps, fear-driven movement).

⚠️ Student/indie demo. Systems are focused, self-contained, and easy to read.

✨ Features

🧠 AI States: Patrol ➜ Chase when player seen; returns to patrol after loss

👂 Noise System: Footstep radius scales with walk / crouch / sprint

👀 Sight System: Spotlight cone & line-of-sight → fear buildup + chase

🧳 Interaction: Physics grab/hold/throw (RMB), contextual UI prompts

🫨 Immersion: Head-bob, footsteps with random pitch, fear slows player

🎧 Audio: Spatialized sources, mixer-driven volume sliders (log scale)

🎬 Capture/Jumpscare: Animation-timed kill with camera takeover

🕹️ Controls

WASD move · Mouse look

L-Shift sprint · L-Ctrl crouch

RMB grab/hold movable object (release to drop/throw)

ESC pause (if present in scene)

🗂️ Project Tree (scripts of interest)
/Characters
  AIMovementController.cs
  PatrolAndFollow.cs
  PlayerBehaviour.cs
  PriestNoiseCheck.cs
  PriestVisibilityCheck.cs
  HeadBobSystem.cs
  Footsteps.cs
  SpotlightController.cs
/Audio
  AudioManager.cs   (Unity AudioMixer)

🚀 Getting Started

Clone and open in Unity 2021.3+ LTS (Cinemachine package recommended).

Ensure layers exist: Player, Walls, MovableObject.

Bake a NavMesh (AI uses NavMeshAgent).

Assign AudioMixer to AudioManager (exposes Music and Sounds).

Press Play. Hide. Listen. Run.

🧩 Core Systems (real snippets)
🎮 Player — movement, fear-based speed, physics grab
// PlayerBehaviour.cs (selected)
void CamRotationAndMovement()
{
    Vector3 move = transform.forward * Input.GetAxisRaw("Vertical")
                 + transform.right   * Input.GetAxisRaw("Horizontal");

    float fearPct = (FearManager.instance.currentFear / FearManager.instance.maxFear);
    float modifiedSpeed = speed - (speed * fearPct * maxSpeedReductionPercentage);

    if (Ccontroller && Ccontroller.enabled && !GameManager.instance.Death)
        Ccontroller.SimpleMove(move * modifiedSpeed);

    SetRotation(); // mouse look with clamps
}

// RMB pick up & hold MovableObject (layer)
if (Physics.Raycast(new Ray(MainCamera.transform.position, MainCamera.transform.forward),
                    out var hit, 10f, 1 << LayerMask.NameToLayer("MovableObject")))
{
    if (Input.GetMouseButtonDown(1)) { hitRigidbody = hit.rigidbody; /* gravity/kinematic set */ }
}
if (hitRigidbody)
{
    Vector3 target = MainCamera.transform.position + MainCamera.transform.forward * baseDistance;
    hitRigidbody.velocity = (target - hitRigidbody.position) * 10f;
    if (Input.GetMouseButtonUp(1)) { /* drop/impulse + restore */ }
}


Crouch / Sprint also change footstep loudness & noise radius:

// crouch
stepVolume = 0.3f; playerVolumeCollider.radius = 0.75f;
// sprint
stepVolume = 1f;   playerVolumeCollider.radius = 4f;

👣 Footsteps — random-pitch, rate from player
// Footsteps.cs (selected)
if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && GameManager.instance.outTaxi)
{
    if (stepCoolDown <= 0f)
    {
        footSteps.pitch = 1f + Random.Range(-0.2f, 0.2f);
        footSteps.PlayOneShot(footStepsClips_Pavement[Random.Range(0, footStepsClips_Pavement.Length)],
                              player.stepVolume);
        stepCoolDown = player.stepRate; // sprint/crouch tweak this
    }
}

🧠 AI — Patrol ↔ Follow, chase audio, capture sequence
// PatrolAndFollow.cs (selected)
enum AIstates { PATROL, FOLLOW }
void HandlePatrolState()
{
    if (agent.remainingDistance <= 2.5f) NextPatrolPoint();
    if (priestVision.playerInView) { ChaseStart(); }
}

void HandleFollowState()
{
    if (!priestVision.playerInView) { FadeChaseAndMaybeReturnToPatrol(); }
    Vector3 toPlayer = playerPos.position - agent.transform.position;
    if (toPlayer.sqrMagnitude >= maxDistance * maxDistance) agent.SetDestination(playerPos.position);
    else StartCoroutine(GrabAndKill()); // stop agent, play anim, take camera
}

🔦 Spotlight Vision — multi-ray cone + walls occlusion
// SpotlightController.cs (selected)
for (float ax = -angle + addAngleX; ax <= angle + addAngleX; ax += seperation)
for (float ay = -angle + addAngleY; ay <= angle + addAngleY; ay += seperation)
{
    var rot = Quaternion.AngleAxis(ay, Vector3.up) * Quaternion.AngleAxis(ax, Vector3.right);
    var dir = rot * thisLight.transform.forward;
    if (Physics.Raycast(thisLight.transform.position, dir, out var hit, maxDistance, wallsMask | playerMask))
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls")) continue;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) playerInLightRay = true;
    }
}

if (playerInLightRay)
{
    priestAnimation.SetBool("PlayerInView", true);
    priest.transform.LookAt(player.position);
    priestVision.spotlightView = priestVision.playerInView = true;
    FearManager.instance.ModifyFear(50f * Time.deltaTime); // fear ramps while seen
}

🎧 Audio Mixer — logarithmic sliders (no clipping)
// AudioManager.cs (selected)
public void SetSoundVolume(float v)
{
    v = Mathf.Clamp(v, 0.0001f, 1f);
    audioMixer.SetFloat("Sounds", Mathf.Log10(v) * 20f);
}

🧠 Design Notes

Sight beats Noise: direct line-of-sight triggers chase; noise helps lure/pressure

Fear ↘ movement: up to 50% speed reduction at max fear (tension curve)

Cinemachine camera for smooth grabs & jumpscare framing (optional)

Performance: vision cone uses coarse ray grid; keep angles/steps modest

🐞 Known Limits

Prototype AI (2 states); expand with search/investigate if desired

Vision cone is ray-based (no GPU occlusion); tune rays for perf

Physics grab is simple velocity target — avoid very heavy props

🛣️ Next Steps (nice-to-haves)

🔎 “Investigate last seen” state with breadcrumb timer

🕳️ Sound emitters for thrown items to mislead AI

🧭 Minimap/heartbeat when close to seeker

💾 Save checkpoints & settings (sens/volumes)
