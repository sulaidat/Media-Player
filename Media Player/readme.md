# A. Core requirements (7 points)

- [x] Add all media files you want to play into a playlist
- [x] Add and remove files from the playlist
- [x] Save and load a playlist
- [ ] Show the current progress of the playing file, allow seeking
- [x] Play in shuffle mode
- [x] Play the next file in playlist, play the previous file in the playlist

# B. Suggested improvement (3 points)

- [x] Store recently played files
- [ ] Keep last played position for continuous viewing
- [x] Support both audio and video files (choose your own favorite popular formats like mp3, flv, mpg)
- [ ] Display preview when seeking
- [ ] Add hooking to support global shortcut key for pause / play / skip to next file

### my checklist

- [x] responsive ui
  - [ ] if media is mp3
    - [ ] set icon as thumbnail by default
    - [ ] else set title as thumbnail
  - [ ] Show first frame of video in WPF MediaElement
  
- [x] switch between Play and Pause button on click
- [x] buttons
  - [x] previous + next media
  - [x] open playlist
  - [x] toggle loop button
    - [x] loop 
    - [x] loop one
  - [x] shuffle
  - [x] volume
    - [ ] scroll to volume up
    - [ ] reset volume
  - [x] speed
    - [ ] scroll to speed up
    - [ ] reset speed
- [ ] timeline slider
  - [x] video frame come up with slider (while played only)
  - [x] change media position by 
    - [x] dragging thumb 
    - [x] or clicking on track
    - [x] can continue dragging after clicking
      - [x] even with mouse leaving the thumb
- [ ] playlist window 
  - [x] add, remove playlist
    - [ ] auto name new playlist 
  - [x] add, remove medias of a playlist
    - [x] select multiple medias
    - [x] drag files to add medias to playlist
    - [x] check file extension before adding
    - [x] check duplicate when adding
  - [x] context menu
    - [x] rename
      - [x] select all text when focus on texbox
    - [x] import, export playlist
  - [x] autosave playlist locally
  - [x] load playlists from `playlists.json`
    - [x] remove non existing media after loaded (in case `playlists.json` is modified unintendedly)
  - [x] force play when double click media from playlist window 
  - [ ] display which media and playlist is currently playing
  - [ ] listview cannot scroll ???
- [ ] Save and load last state
  - [ ] playlist and media index
  - [ ] media position
  - [ ] modes 

- [ ] keyboard hook
  - [ ] go forward x seconds
  - [ ] go backward x seconds

