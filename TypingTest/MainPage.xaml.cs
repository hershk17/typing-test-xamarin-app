using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TypingTest
{
    public partial class MainPage : ContentPage
    {
        private const int MAXWORDS = 80;

        public string[] words = new string[MAXWORDS];

        public string[] prevWords;
        public string currWord;
        public string[] afterWords;

        public int position = -1;

        bool skipInputChangeEvent = false;

        public MainPage()
        {
            InitializeComponent();

            string content;
            AssetManager assets = Android.App.Application.Context.Assets;
            using (StreamReader reader = new StreamReader(assets.Open("words.txt")))
            {
                content = reader.ReadToEnd();
            }

            string[] unfilteredWords = content.Split();

            Random r = new Random();
            unfilteredWords = unfilteredWords.OrderBy(x => r.Next()).ToArray();

            for(int i = 0, j = 0; i < unfilteredWords.Length && j < 80; i++)
            {
                if(unfilteredWords[i] != "")
                {
                    words[j] = unfilteredWords[i];
                    j++;
                }
            }

            NextWord();
        }

        public void NextWord()
        {
            position++;

            prevWords = new string[position];
            for (int i = 0; i < position; i++)
            {
                prevWords[i] = words[i];
            }

            currWord = words[position];

            afterWords = new string[MAXWORDS - position - 1];
            for (int i = position + 1, j = 0; i < MAXWORDS; i++, j++)
            {
                afterWords[j] = words[i];
            }

            wordsList.Text = "<p>" + string.Join(" ", prevWords) + " <b><u>" + currWord + "</u></b> " + string.Join(" ", afterWords) + "</p>";
        }

        private void entryField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!skipInputChangeEvent)
            {
                if (entryField.Text[entryField.Text.Length - 1] == ' ')
                {
                    NextWord();
                    entryField.Text = "";
                    skipInputChangeEvent = true;
                }
            }
            else {
                skipInputChangeEvent = false;
            }
        }
    }
}
