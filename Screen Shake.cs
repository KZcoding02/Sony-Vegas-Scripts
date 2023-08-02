using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ScriptPortal.Vegas;

public class EntryPoint {

    public void FromVegas(Vegas vegas) {
        VideoEvent videoEvent = FindSelected(vegas.Project);
        ScreenShaker(vegas, videoEvent);
    }

    void ScreenShaker(Vegas vegas, VideoEvent videoEvent) {
		// shake pattern moves top left, bot right, bot left, top right
		int videoWidth = vegas.Project.Video.Width;
		int videoHeight = vegas.Project.Video.Height;
		int shakeFactor = 30; // reciprical of intensity of shake; vertex movement% = [width or height]/shakefactor
        
		for (int i = 0; i < 12; i++) {
			VideoMotionKeyframe key1 = new VideoMotionKeyframe(Timecode.FromSeconds(i*0.0166));
        	videoEvent.VideoMotion.Keyframes.Add(key1);
			VideoMotionKeyframe key0 = videoEvent.VideoMotion.Keyframes[i];
			if (i == 0) {
				key0.MoveBy(new VideoMotionVertex(-1*videoWidth/shakeFactor, -1*videoHeight/shakeFactor));
				continue;
			}
			int frame = i%4;
			switch (frame) {
				case 0:
					key0.MoveBy(new VideoMotionVertex(-2*videoWidth/shakeFactor, 0));
					break;
				case 1:
					key0.MoveBy(new VideoMotionVertex(2*videoWidth/shakeFactor, 2*videoHeight/shakeFactor));
					break;
				case 2:
					key0.MoveBy(new VideoMotionVertex(-2*videoWidth/shakeFactor, 0));
					break;
				case 3:
					key0.MoveBy(new VideoMotionVertex(2*videoWidth/shakeFactor, -2*videoHeight/shakeFactor));
					break;
			}
		}
		VideoMotionKeyframe keyLast = new VideoMotionKeyframe(Timecode.FromSeconds(12*0.0166));
        videoEvent.VideoMotion.Keyframes.Add(keyLast);
		VideoMotionKeyframe keyReset = videoEvent.VideoMotion.Keyframes[12];
		keyReset.MoveBy(new VideoMotionVertex(-1*videoWidth/shakeFactor, videoHeight/shakeFactor));
    }

    VideoEvent FindSelected(Project project) {
		VideoEvent casket = new VideoEvent();
		foreach (Track track in project.Tracks) {
			if (track.IsVideo()) {
					foreach (VideoEvent videoEvent in track.Events)
					{
						if (videoEvent.Selected)
						{
							return videoEvent;
						}
					}
			  }
		}
		return casket;
	}
}