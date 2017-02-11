using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int gameWidth = 10;
        private int gameHeight = 15;
        private int[,] bgGround = new int[15, 10];
        //活动的方块
        private int[,,,] activeBlocks = {{
                                     {
                                         {1,0,0,0},
                                         {1,0,0,0},
                                         {1,0,0,0},
                                         {1,0,0,0}
                                     },
                                     {
                                         {1,1,1,1},
                                         {0,0,0,0},
                                         {0,0,0,0},
                                         {0,0,0,0}
                                     },
                                     {
                                         {1,0,0,0},
                                         {1,0,0,0},
                                         {1,0,0,0},
                                         {1,0,0,0}
                                     },
                                     {
                                         {1,1,1,1},
                                         {0,0,0,0},
                                         {0,0,0,0},
                                         {0,0,0,0}
                                     }
                                 },
                                 {
                                      {
                                          {1,1,0,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,1,0,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,1,0,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,1,0,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      }
                                  },
                                  {
                                      {
                                          {1,0,0,0},
                                          {1,1,0,0},
                                          {0,1,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {0,1,1,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,0,0,0},
                                          {1,1,0,0},
                                          {0,1,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {0,1,1,0},
                                          {1,1,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      }
                                  },
                                  {
                                      {
                                          {1,1,0,0},
                                          {0,1,0,0},
                                          {0,1,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {0,0,1,0},
                                          {1,1,1,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,0,0,0},
                                          {1,0,0,0},
                                          {1,1,0,0},
                                          {0,0,0,0}
                                      },
                                      {
                                          {1,1,1,0},
                                          {1,0,0,0},
                                          {0,0,0,0},
                                          {0,0,0,0}
                                      }
                                  }};
        private int TricksNum = 4;
        private int StatusNum = 4;
        private int CurrentTrickNum;
        private int CurrentStatusNum;
        private int[,] CurrentTrick = new int[4, 4];
        private int CurrentX;
        private int CurrentY;

        public MainWindow()
        {
            InitializeComponent();
            this.initGrid();
            this.BeginTricks();
            this.cronActive();

        }

        

        private void initGrid()
        {
            this.gameArea.Background = Brushes.White;
            for (int i = 0; i < this.gameHeight; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                this.gameArea.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < this.gameWidth; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                this.gameArea.ColumnDefinitions.Add(columnDefinition);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BeginTricks()
        {
            //随机生成活动方块
            Random rand = new Random();
            this.CurrentTrickNum = rand.Next(0, this.TricksNum);
            this.CurrentStatusNum = rand.Next(0, StatusNum);
            //分配数组  
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    this.CurrentTrick[y, x] = this.activeBlocks[this.CurrentTrickNum, this.CurrentStatusNum, y, x];
                }
            }
            this.CurrentX = 0;
            this.CurrentY = 0;
            this.drawActionBlocks();
            
        }




        /// <summary>
        /// 画出活动的方块
        /// </summary>
        private void drawActionBlocks()
        {
            this.gameArea.Children.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //Console.Write(this.CurrentTrick[i, j]);
                    Rectangle rtg = new Rectangle();
                    rtg.Width = 30;
                    rtg.Height = 30;
                    SolidColorBrush myBrush = new SolidColorBrush(Colors.Green);
                    rtg.Fill = myBrush;
                    Grid.SetColumn(rtg, i+this.CurrentX);
                    Grid.SetRow(rtg, j+this.CurrentY);
                    if (this.CurrentTrick[i, j] == 1)
                    {
                        this.gameArea.Children.Add(rtg);
                    }
                }
            }
            //画出底部的方块
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //Console.Write(this.CurrentTrick[i, j]);
                    Rectangle rtg = new Rectangle();
                    rtg.Width = 30;
                    rtg.Height = 30;
                    SolidColorBrush myBrush = new SolidColorBrush(Colors.Blue);
                    rtg.Fill = myBrush;
                    Grid.SetRow(rtg, i);
                    Grid.SetColumn(rtg, j);
                    if (this.bgGround[i, j] == 1)
                    {
                        this.gameArea.Children.Add(rtg);
                    }
                }
            }
        }

    

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.D))
            {
                this.checkIsRight();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.A))
            {
                this.checkIsLeft();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.S))
            {
                this.checkIsDown();
            }
            else if (e.KeyStates == Keyboard.GetKeyStates(Key.Z))
            {
                int actionHeight = this.getActionMaxHeight();
                if (this.CurrentY >= this.gameHeight - actionHeight)
                {
                    return;
                }
                this.ChangeTricks();
            }
            this.drawActionBlocks();
        }

        private void ChangeTricks()
        {
            if (CurrentStatusNum < 3)
            {
                CurrentStatusNum++;
            }
            else
            {
                CurrentStatusNum = 0;
            }
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    CurrentTrick[y, x] = this.activeBlocks[this.CurrentTrickNum, this.CurrentStatusNum, y, x];
                }
            }
        }

        private void checkIsRight()
        {
            //活动方块的最大宽度
            int actionWidth = this.getActionMaxWidth();
            int actionHeight = this.getActionMaxHeight();
            Console.WriteLine("最大宽度" + actionWidth);
            Console.WriteLine("最大高度" + actionHeight);
            if (this.CurrentX >= this.gameWidth - actionWidth)
            {
                this.CurrentX = this.gameWidth - actionWidth;
                return;
            }
            this.CurrentX++;
        }

        private void checkIsLeft()
        {
            if (this.CurrentX == 0)
            {
                return;
            }
            this.CurrentX--;
        }

        private bool CheckCanDown()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (CurrentTrick[y, x] == 1)
                    {
                        //超过了背景  
                        if (y + CurrentY + 1 >= 15)
                        {
                            return false;
                        }
                       
                        if (this.bgGround[y + CurrentY + 1, x + CurrentX] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void checkIsDown()
        {
            //活动方块的最大宽度
            int actionWidth = this.getActionMaxWidth();
            int actionHeight = this.getActionMaxHeight();
            Console.WriteLine("最大宽度" + actionWidth);
            Console.WriteLine("最大高度" + actionHeight);
            

            if (this.CurrentY >= this.gameHeight - actionHeight)
            {
                this.CurrentY = this.gameHeight - actionHeight;
                this.downAction();
                return;
            }
            //边界检查有点难弄，需要重新考虑。
            //for (int x = 0; x < 4; x++)
            //{
            //    for (int y = 0; y < 4; y++)
            //    {
            //        if ((y + CurrentY + 1) < this.gameHeight && this.bgGround[y + CurrentY + 1, x + CurrentX] == 1)
            //        {
            //            this.CurrentY = y + this.CurrentY - actionHeight + 1 ;
            //            this.downAction();
            //            return;
            //        }
            //    }
            //}
            this.CurrentY++;
        }

        private void downAction()
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (this.CurrentTrick[x, y] == 1)
                    {
                        this.bgGround[y + this.CurrentY, x + this.CurrentX] = 1;
                    }
                }
            }
            //统计满行行数，位置
            int totalNum = 0;
            ArrayList arrRow = new ArrayList();
            for (int i = 0; i < this.gameHeight; i++)
            {
                bool isFull = true;
                for (int j = this.gameWidth - 1; j > 0; j--)
                {
                    if (this.bgGround[i, j] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                if (isFull == true)
                {
                    totalNum++;
                    arrRow.Add(i);
                }
            }
            //消除满行
            if (totalNum > 0)
            {
                foreach (int last in arrRow)
                {
                    Console.WriteLine("消除的行数是：" + last);
                    for (int yy = last; yy > 0; yy--)
                    {
                        for (int xx = 0; xx < this.gameWidth; xx++)
                        {
                            int temp = this.bgGround[yy - 1, xx];
                            this.bgGround[yy, xx] = temp;
                        }
                    }
                }
            }
            //总结分数
            if (totalNum == 1)
            {
                this.scoreBlock.Text = (Convert.ToInt32(this.scoreBlock.Text) + 100).ToString();
            }
            else if (totalNum == 2)
            {
                this.scoreBlock.Text = (Convert.ToInt32(this.scoreBlock.Text) + 300).ToString();
            }
            else if (totalNum == 3)
            {
                this.scoreBlock.Text = (Convert.ToInt32(this.scoreBlock.Text) + 500).ToString();
            }
            else if (totalNum == 4)
            {
                this.scoreBlock.Text = (Convert.ToInt32(this.scoreBlock.Text) + 1000).ToString();
            }
            this.BeginTricks();
        }



        /// <summary>
        /// 获取活动方块的最大宽度
        /// </summary>
        /// <returns></returns>
        private int getActionMaxWidth()
        {
            int a = 0;
            for (int x = 0; x < 4; x++)
            {
                bool isHave = false;
                for (int y = 0; y < 4; y++)
                {
                    if (this.CurrentTrick[x, y] == 1 && isHave==false)
                    {
                        isHave = true;
                        break;
                    }
                }
                if (isHave == true)
                {
                    a++;
                    continue;
                }
            }
            return a;
        }

        /// <summary>
        /// 获取活动方块的最大高度
        /// </summary>
        /// <returns></returns>
        private int getActionMaxHeight()
        {
            int a = 0;
            for (int x = 0; x < 4; x++)
            {
                bool isHave = false;
                for (int y = 0; y < 4; y++)
                {
                    if (this.CurrentTrick[y, x] == 1 && isHave == false)
                    {
                        isHave = true;
                        break;
                    }
                }
                if (isHave == true)
                {
                    a++;
                    continue;
                }
            }
            return a;
        }

        /// <summary>
        /// 定时向下移动
        /// </summary>
        private void cronActive()
        {
            DispatcherTimer readDataTimer = new DispatcherTimer();
            readDataTimer.Tick += new EventHandler(downAction);
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 1);
            readDataTimer.Start();
        }

        private void downAction(object sender, EventArgs e)
        {
            this.checkIsDown();
            this.drawActionBlocks();            
        }

    }
}
