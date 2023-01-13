﻿using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for PlaylistWindow.xaml
    /// </summary>
    public partial class PlaylistWindow : Window
    {
        ObservableCollection<Playlist> _playlists { get; set; }
        public EventHandler<MediaSelectedArgs> MediaSelected;
        static PlaylistWindow _instance;
        MediaSelectedArgs _currentMediaInfo;


        public static PlaylistWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlaylistWindow();
            }
            return _instance;
        }

        public PlaylistWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlaylistFromFile("playlists.json");
            RemoveNonexistingMediaFromPlaylists();

            _currentMediaInfo = new MediaSelectedArgs();
            _currentMediaInfo.PropertyChanged += HighlightCurrentMedia;
        }

        private void HighlightCurrentMedia(object? sender, PropertyChangedEventArgs e)
        {
            var oldPlaylistIndex = _currentMediaInfo.OldPlaylistIndex;
            var oldMediaIndex = _currentMediaInfo.OldMediaIndex;
            var newPlaylistIndex = _currentMediaInfo.PlaylistIndex;
            var newMediaIndex = _currentMediaInfo.MediaIndex;
            _currentMediaInfo.OldPlaylistIndex = newPlaylistIndex;
            _currentMediaInfo.OldMediaIndex = newMediaIndex;

            // Unhighlight old media
            _playlists[oldPlaylistIndex].IsPlaying = false;
            _playlists[oldPlaylistIndex].List[oldMediaIndex].IsPlaying = false;

            // Highlight current media
            _playlists[newPlaylistIndex].IsPlaying = true;
            _playlists[newPlaylistIndex].List[newMediaIndex].IsPlaying = true;
        }

        private void RemoveNonexistingMediaFromPlaylists()
        {
            foreach (var playlist in _playlists)
            {
                for (var i = 0; i < playlist.List.Count; i++)
                {
                    if (!File.Exists(playlist.List[i].Path))
                    {
                        playlist.List.RemoveAt(i);
                    }
                }
            }
        }

        private void LoadPlaylistFromFile(string filename)
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directory, filename);
            if (File.Exists(filePath))
            {
                _playlists = JsonConvert.DeserializeObject<ObservableCollection<Playlist>>(File.ReadAllText(filePath));
            }
            if (_playlists == null)
            {
                _playlists = new ObservableCollection<Playlist>();
            }
            // register SavePlaylists hook
            _playlists.CollectionChanged += SavePlaylists;
            foreach (var playlist in _playlists)
            {
                playlist.PropertyChanged += SavePlaylists;
                playlist.List.CollectionChanged += SavePlaylists;
                foreach (var media in playlist.List)
                {
                    media.PropertyChanged += SavePlaylists;
                }
            }

            listview_playlist.ItemsSource = _playlists;
        }

        private void SavePlaylists(object? sender, PropertyChangedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(_playlists);
            File.WriteAllText("playlists.json", json);
        }

        private void SavePlaylists(object? sender, NotifyCollectionChangedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(_playlists);
            File.WriteAllText("playlists.json", json);
        }

        public void SendData(MediaSelectedArgs args)
        {
            _currentMediaInfo = args;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listview_playlist.SelectedIndex != -1)
            {
                datagrid_medias.ItemsSource = _playlists[listview_playlist.SelectedIndex].List;
                btn_addMedia.Visibility = Visibility.Visible;
            }
        }

        private void OnClick_NewPlaylist(object sender, RoutedEventArgs e)
        {
            var newPlaylist = new Playlist();
            newPlaylist.Name = "Playlist";
            newPlaylist.List = new ObservableCollection<Media>();
            newPlaylist.IsPlaying = false;
            newPlaylist.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
            _playlists.Add(newPlaylist);
        }

        private void OnClick_AddMedia(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "Media files (*.mp3;*.mp4;*flv;*.mpg)|*.mp3;*.mp4;*flv;*.mpg|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() ?? false)
            {
                foreach (var filePath in dialog.FileNames)
                {
                    AddMedia(filePath);
                }
            }
        }

        private void AddMedia(string? filePath)
        {
            // find duplicate
            var res = _playlists[listview_playlist.SelectedIndex].List.Where(media => media.Path == filePath);
            if (res.Any())
            {
                return;
            }
            var newMedia = new Media();
            newMedia.Path = filePath;
            newMedia.Name = Path.GetFileName(filePath);
            newMedia.IsPlaying = false;
            newMedia.PropertyChanged += SavePlaylists;

            _playlists[listview_playlist.SelectedIndex].List.Add(newMedia);
        }

        private void OnClick_RenamePlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new PlaylistRenameWindow(_playlists[listview_playlist.SelectedIndex]);
            if (dialog.ShowDialog() ?? false)
            {
                _playlists[listview_playlist.SelectedIndex].Name = dialog.Playlist.Name;
            }
        }

        private void OnClick_RemovePlaylist(object sender, RoutedEventArgs e)
        {
            _playlists.RemoveAt(listview_playlist.SelectedIndex);
            datagrid_medias.ItemsSource = null;
        }

        private void OnClick_RemoveMedia(object sender, RoutedEventArgs e)
        {
            _playlists[listview_playlist.SelectedIndex].List.RemoveAt(datagrid_medias.SelectedIndex);
        }

        private void OnMouseDoubleClick_PlayMedia(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _currentMediaInfo.PlaylistIndex = listview_playlist.SelectedIndex;
            _currentMediaInfo.MediaIndex = datagrid_medias.SelectedIndex;
            _currentMediaInfo.Playlist = _playlists[listview_playlist.SelectedIndex].List;

            MediaSelected?.Invoke(this, _currentMediaInfo);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        private void OnDrop_AddMedias(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var filePath in filePaths)
                {
                    var ext = Path.GetExtension(filePath);
                    if (String.Equals(ext, ".mp3") || String.Equals(ext, ".mp4"))
                    {
                        AddMedia(filePath);
                    }
                }
            }
        }

        private void OnClick_ExportSinglePlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() ?? false)
            {
                string json = JsonConvert.SerializeObject(_playlists[listview_playlist.SelectedIndex]);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void OnClick_ExportAllPlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() ?? false)
            {
                string json = JsonConvert.SerializeObject(_playlists);
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private void OnClick_ImportSinglePlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() ?? false)
            {
                var newPlaylist = JsonConvert.DeserializeObject<Playlist>(File.ReadAllText(dialog.FileName));
                newPlaylist.PropertyChanged += SavePlaylists;
                newPlaylist.List.CollectionChanged += SavePlaylists;
                _playlists.Add(newPlaylist);
            }
        }

        private void OnClick_ImportMultiplePlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() ?? false)
            {
                var newPlaylists = JsonConvert.DeserializeObject<ObservableCollection<Playlist>>(File.ReadAllText(dialog.FileName));
                foreach (var playlist in newPlaylists)
                {
                    playlist.PropertyChanged += SavePlaylists;
                    playlist.List.CollectionChanged += SavePlaylists;
                    _playlists.Add(playlist);
                }
            }
        }
    }

    public class MediaSelectedArgs: EventArgs, INotifyPropertyChanged
    {
        int _playlistIndex;
        int _mediaIndex;

        public ObservableCollection<Media> Playlist { get; set; }
        public int PlaylistIndex
        {
            get
            {
                return _playlistIndex;
            }
            set
            {
                _playlistIndex = value;
                EmitPropertyChanged();
            }
        }
        public int MediaIndex
        {
            get
            {
                return _mediaIndex;
            }
            set
            {
                _mediaIndex = value;
                EmitPropertyChanged();
            }
        }
        public int OldPlaylistIndex { get; set; }
        public int OldMediaIndex { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void EmitPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
