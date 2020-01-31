namespace CJStudio.Dash.Camera {
    using System.Threading.Tasks;

    using Cinemachine;

    using UnityEngine;
    [System.Serializable]
    class CameraController {
        [SerializeField] CinemachineClearShot shot = null;
        [SerializeField] CameraShakeProps shakeProps = null;
        //CinemachineVirtualCamera shakingCamera = null;
        async public void ShakeCamera (CameraShakeProps shakeProps = null) {
            CameraShakeProps props = shakeProps == null?this.shakeProps : shakeProps;
            if (shot.LiveChild != null) {
                CinemachineVirtualCamera cam = shot.LiveChild as CinemachineVirtualCamera;
                CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ( );
                if (noise != null) {
                    noise.m_AmplitudeGain = props.Amplitude;
                    noise.m_FrequencyGain = props.Frequency;
                    await Task.Delay (props.ShakeDuration);
                    noise.m_AmplitudeGain = 0f;
                    noise.m_FrequencyGain = 0f;
                }
            }
        }

        public void SetCameraShake (CameraShakeProps shakeProps) {
            if (shot.LiveChild != null) {
                CinemachineVirtualCamera cam = shot.LiveChild as CinemachineVirtualCamera;
                CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ( );
                if (noise != null) {
                    noise.m_AmplitudeGain = shakeProps.Amplitude;
                    noise.m_FrequencyGain = shakeProps.Frequency;
                }
            }
        }

        public void DisableCameraShake ( ) {
            if (shot.LiveChild != null) {
                CinemachineVirtualCamera cam = shot.LiveChild as CinemachineVirtualCamera;
                CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ( );
                if (noise != null) {
                    noise.m_AmplitudeGain = 0f;
                    noise.m_FrequencyGain = 0f;
                }
            }
        }
    }

    [System.Serializable]
    class CameraShakeProps {
        [SerializeField] float amplitude = 0f;
        [SerializeField] float frequency = 0f;
        [SerializeField] [Tooltip ("MilliSecond")] int shakeDuration = 500;
        public float Amplitude => amplitude;
        public float Frequency => frequency;
        public int ShakeDuration => shakeDuration;
        public CameraShakeProps (float amplitude, float frequency, int shakeDuration) {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.shakeDuration = shakeDuration;
        }
    }
}
