using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{

    protected class PlateauJeux
    {
        private int _coordonneMaxH;
        private int _coordonneMinH = 0;
        private int _coordonneMaxV;
        private int _coordonneMinV = 0;
        private string _hv = string.Empty;

        public int CoordonneMaxH { get => _coordonneMaxH; set => _coordonneMaxH = value; }
        public int CoordonneMinH { get => _coordonneMinH; set => _coordonneMinH = value; }
        public int CoordonneMaxV { get => _coordonneMaxV; set => _coordonneMaxV = value; }
        public int CoordonneMinV { get => _coordonneMinV; set => _coordonneMinV = value; }
        public string Hv { get => _hv; set => _hv = value; }


        public PlateauJeux()
        {
        }

        public PlateauJeux(int w, int h)
        {
            _coordonneMaxH = w - 1;
            _coordonneMaxV = h - 1;
        }

        /// <summary>
        /// calcul le nouveau plateau par rapport au retour de bomDir 
        /// et la possition de Batman
        /// </summary>
        /// <param name="bombDir">position de la bombe par rapport à Batman</param>
        /// <param name="batman">batman lui même avec ces coordonnées sur le plateau</param>
        /// <returns>retourne le nouveau plateau</returns>
        public PlateauJeux Extract(Batman batman)
        {
            PlateauJeux nouveau = new PlateauJeux();
            switch (batman.Zone)
            {
                case "U":   // -- les bombes sont situées au dessus de Batman
                    nouveau.CoordonneMaxH = batman.X;
                    nouveau.CoordonneMinH = batman.X;
                    nouveau.CoordonneMaxV = batman.Y - 1;
                    nouveau.CoordonneMinV = this.CoordonneMinV;
                    nouveau.Hv = "Y";
                    break;
                case "UR":  // -- Up-Right : les bombes sont situées au dessus et à droite de Batman
                    nouveau.CoordonneMaxH = this._coordonneMaxH;
                    nouveau.CoordonneMaxV = batman.Y - 1;
                    nouveau.CoordonneMinH = batman.X + 1;
                    nouveau.CoordonneMinV = this.CoordonneMinV;
                    nouveau.Hv = string.Empty;
                    break;
                case "R":   // -- Right : les bombes sont situées à droite de Batman
                    nouveau.CoordonneMaxH = this.CoordonneMaxH;
                    nouveau.CoordonneMaxV = batman.Y;
                    nouveau.CoordonneMinH = batman.X + 1;
                    nouveau.CoordonneMinV = batman.Y;
                    nouveau.Hv = "X";
                    break;
                case "DR":  // -- Down-Right : les bombes sont situées en dessous et à droite de Batman
                    nouveau.CoordonneMaxH = this.CoordonneMaxH;
                    nouveau.CoordonneMaxV = this.CoordonneMaxV;
                    nouveau.CoordonneMinH = batman.X + 1;
                    nouveau.CoordonneMinV = batman.Y + 1;
                    nouveau.Hv = string.Empty;
                    break;
                case "D":   // -- Down : les bombes sont situées en dessous de Batman
                    nouveau.CoordonneMaxH = batman.X;
                    nouveau.CoordonneMaxV = this.CoordonneMaxV;
                    nouveau.CoordonneMinH = batman.X;
                    nouveau.CoordonneMinV = batman.Y + 1;
                    nouveau.Hv = "Y";
                    break;
                case "DL":  // -- Down-Left : les bombes sont situées en dessous et à gauche de Batman
                    nouveau.CoordonneMaxH = batman.X - 1;
                    nouveau.CoordonneMaxV = this.CoordonneMaxV;
                    nouveau.CoordonneMinH = this.CoordonneMinH;
                    nouveau.CoordonneMinV = batman.Y + 1;
                    nouveau.Hv = string.Empty;
                    break;
                case "L":   // -- Left : les bombes sont situées à gauche de Batman
                    nouveau.CoordonneMaxH = batman.X - 1;
                    nouveau.CoordonneMaxV = batman.Y;
                    nouveau.CoordonneMinH = this.CoordonneMinH;
                    nouveau.CoordonneMinV = batman.Y;
                    nouveau.Hv = "X";
                    break;
                case "UL":  // -- Up-Left : les bombes sont situées au dessus et à gauche de Batman
                    nouveau.CoordonneMaxH = batman.X - 1;
                    nouveau.CoordonneMaxV = batman.Y + 1;
                    nouveau.CoordonneMinH = this.CoordonneMinH;
                    nouveau.CoordonneMinV = this.CoordonneMinV;
                    nouveau.Hv = string.Empty;
                    break;
            }
            return nouveau;
        }
    }

    protected class Batman
    {
        private int _x;
        private int _y;
        private string _zone;
        private static Batman _instance = null;
        private static object _verrou = new object();

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public string Zone { get => _zone; set => _zone = value; }

        private Batman()
        {
        }

        public static Batman CreateBatman
        {
            get
            {
                lock (_verrou)
                {
                    if (_instance == null)
                    {
                        _instance = new Batman();
                    }
                    return _instance;
                }
            }
        }

        public void CalculCoordonnee(PlateauJeux plateau)
        {
            if (string.IsNullOrEmpty(plateau.Hv))
            {
                if (this._zone == "UR" || this._zone == "DR")
                {
                    _x = (plateau.CoordonneMaxH - plateau.CoordonneMinH) / 2 + plateau.CoordonneMinH;
                    _y = _zone == "UR" ? plateau.CoordonneMaxV - ((plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2) : (plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2 + plateau.CoordonneMinV;
                }
                else
                {
                    _x = plateau.CoordonneMaxH - ((plateau.CoordonneMaxH - plateau.CoordonneMinH) / 2);
                    _y = _zone == "UL" ? plateau.CoordonneMaxV - ((plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2) : (plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2 + plateau.CoordonneMinV;
                }
            }
            else
            {
                if (plateau.Hv == "X")
                {
                    if (this._zone == "R")
                    {
                        _x = (plateau.CoordonneMaxH - plateau.CoordonneMinH) / 2 + plateau.CoordonneMinH;
                    }
                    else
                    {
                        _x = plateau.CoordonneMaxH - (plateau.CoordonneMaxH - plateau.CoordonneMinH) / 2;
                    }

                }
                else
                {
                    if (this._zone == "D")
                    {
                        _y = (plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2 + plateau.CoordonneMinV;
                    }
                    else
                    {
                        _y = plateau.CoordonneMaxV - (plateau.CoordonneMaxV - plateau.CoordonneMinV) / 2;
                    }

                }
            }
        }
        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString();
        }
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int W = int.Parse(inputs[0]); // width of the building.
        int H = int.Parse(inputs[1]); // height of the building.

        int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.

        inputs = Console.ReadLine().Split(' ');
        int X0 = int.Parse(inputs[0]);
        int Y0 = int.Parse(inputs[1]);

        PlateauJeux initPlateau = new PlateauJeux()
        {
            CoordonneMaxH = W - 1,
            CoordonneMaxV = H - 1,
        };
        Batman bat = Batman.CreateBatman;
        bat.X = X0;
        bat.Y = Y0;

        // game loop
        while (true)
        {
            string bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            bat.Zone = bombDir;
            initPlateau = initPlateau.Extract(bat);

            bat.CalculCoordonnee(initPlateau);
            Console.Error.WriteLine(bat.ToString());
            // the location of the next window Batman should jump to.
            Console.WriteLine(bat.ToString());
        }
    }
}