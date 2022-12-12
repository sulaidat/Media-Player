# A. Core requirements (7 points)

- [ ] Add all media files you want to play into a playlist
- [ ] Add and remove files from the playlist
- [ ] Save and load a playlist
- [ ] Show the current progress of the playing file, allow seeking
- [ ] Play in shuffle mode
- [ ] Play the next file in playlist, play the previous file in the playlist

# B. Suggested improvement (3 points)

- [ ] Store recently played files
- [ ] Keep last played position for continuous viewing
- [ ] Support both audio and video files (choose your own favorite popular formats like mp3, flv, mpg)
- [ ] Display preview when seeking
- [ ] Add hooking to support global shortcut key for pause / play / skip to next file

### my checklist

- [x] responsive ui
  - [ ] if media is mp3
    - [ ] set icon as thumbnail by default
    - [ ] else set title as thumbnail

- [x] switch between Play and Pause button on click
- [ ] buttons
  - [ ] previous + next media
  - [x] open playlist
  - [ ] loop 
  - [ ] shuffle
  - [ ] volume
  - [ ] speed
- [ ] timeline slider
  - [x] preview come up with slider (while played only)
  - [x] change media position by 
    - [x] dragging thumb 
    - [x] or clicking on track
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
    - [x] import, export playlist
  - [x] autosave playlist locally
  - [x] load playlists from `playlists.json`
    - [x] remove non existing media after loaded (in case `playlists.json` is modified unintendedly)
  - [x] force play when double click media from playlist window 
  - [ ] display which media and playlist is currently playing
  - [ ] listview cannot scroll ???
- [ ] keyboard hook
  - [ ] go forward x seconds
  - [ ] go backward x seconds

