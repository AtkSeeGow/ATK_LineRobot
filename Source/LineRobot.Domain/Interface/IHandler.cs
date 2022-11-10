using Line;
using System.Threading.Tasks;

namespace LineRobot.Domain.Interface
{
    /// <summary>
    /// 事件處理介面
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 事件類型
        /// </summary>
        LineEventType LineEventType { get; }

        /// <summary>
        /// 處理事件
        /// </summary>
        /// <param name="lineBot"></param>
        /// <param name="lineEvent"></param>
        /// <returns></returns>
        Task Handle(ILineBot lineBot, ILineEvent lineEvent);
    }
}