using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 
  public   class ScoreController
    {

        private static ScoreController _instance;

        public static ScoreController Instance
        {
            get { 
                if(_instance==null)
                {
                    _instance = new ScoreController();
                }
                return _instance;
            }
            
        }

      /// <summary>
      /// 根据总消灭个数进行单个积分计算
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
        public int ScoreWithStarNumber(int number)
        {
            return   number * 5;
        }
      /// <summary>
      /// 根据个数算总积分
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
        public int ScoreWithNumber(int number)
        {
            return number * number * 5;
        }
      /// <summary>
      /// 根据最后剩下猫进行积分奖励
      /// </summary>
      /// <param name="number"></param>
      /// <returns></returns>
        public int BonusScoreWithRemain(int number)
        {
            return Math.Max(0, 2000 - number * number * 20);
        }
        public long GetTargetScore(int stage)
        {
            switch (stage)
            {
                case 1:
                    return 1000L;
                case 2:
                    return 3000L;
                case 3:
                case 4:
                case 5:
                    return (long)(6000 + (stage - 3) * 2000);
                case 6:
                case 7:
                case 8:
                    return (long)(13000 + (stage - 6) * 2000);
                default:
                    return (long)(20000 + (stage - 9) * 4000);
            }
        }
    }
 
