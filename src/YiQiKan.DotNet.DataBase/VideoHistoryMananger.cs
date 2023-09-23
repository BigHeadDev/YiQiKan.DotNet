using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiQiKan.DotNet.DataBase.DBContexts;
using YiQiKan.DotNet.Model.DBModel;

namespace YiQiKan.DotNet.DataBase {
    public class VideoHistoryMananger {
        private readonly VideoDBContext videoDBContext;
        public bool InitSucess { get; set; } = false;

        public async Task<bool> InitDataBase() {
            try {
                await videoDBContext.Database.EnsureCreatedAsync();
                InitSucess = true;
            } catch (Exception ex) {
                InitSucess = false;
            }
            return InitSucess;
        }
        public VideoHistoryMananger(VideoDBContext videoDBContext) {
            this.videoDBContext = videoDBContext;
        }

        public async Task<bool> Reset() {
            var all = videoDBContext.VideoHistories;
            videoDBContext.VideoHistories.RemoveRange(all);
            return (await videoDBContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> InsertOrUpdateHistory(VideoHistory history) {
            if (InitSucess) {
                var item = await videoDBContext.VideoHistories.FirstOrDefaultAsync(h => h.MovieId.Equals(history.MovieId));
                if (item == null) {
                    await videoDBContext.VideoHistories.AddAsync(history);
                } else {
                    item.Duration = history.Duration;
                    item.TypeName = history.TypeName;
                    item.ItemIndex = history.ItemIndex;
                    videoDBContext.VideoHistories.Update(item);
                }
                return (await videoDBContext.SaveChangesAsync()) > 0;
            }
            return false;
        }

        public VideoHistory GetHistoryByMovieId(string movieId) {
            if (InitSucess) {
                return videoDBContext.VideoHistories.FirstOrDefault(v => v.MovieId.Equals(movieId));
            }
            return null;
        }
    }
}
