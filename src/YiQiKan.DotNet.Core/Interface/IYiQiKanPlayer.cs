using System;
using System.Collections.Generic;
using System.Text;
using YiQiKan.DotNet.Model.ResponseModel;

namespace YiQiKan.DotNet.Core.Interface {
    public interface IYiQiKanPlayer {
        void Play();

        void Pause();

        void SetSource(PlayListResource playList);

        void SetIndex(int index);
        void SetPosition(long position);
        void Dispose();

        long TotalDuration { get; }
        long CurrentDuration { get; }
        PlayListResource PlayList { get; }
        PlayInfo CurrentPlayInfo { get; }

        event Action<int> IndexChanged;
        event Action MediaOpened;
    }
}
