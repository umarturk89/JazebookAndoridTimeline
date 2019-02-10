using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using FFImageLoading.Views;
using Microsoft.AppCenter.Crashes;
using WoWonder.Helpers;

namespace WoWonder.Activities.AddPost.Adapters
{
    public class PollAnswers
    {
        public int id { get; set; }
        public string Answer { get; set; }
    }

    public class AddPollAdapter : RecyclerView.Adapter
    {
        public int Position;
        public Context Activity_Context;
        public ObservableCollection<PollAnswers> AnswersList = new ObservableCollection<PollAnswers>();

        public AddPollAdapter(Context context)
        {
            try
            {
                Activity_Context = context;
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }


        public override int ItemCount
        {
            get
            {
                if (AnswersList != null)
                    return AnswersList.Count;
                return 0;
            }
        }

        public event EventHandler<AddPollAdapterClickEventArgs> ItemClick;
        public event EventHandler<AddPollAdapterClickEventArgs> ItemLongClick;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_Gif_View
                var itemView = LayoutInflater.From(parent.Context) .Inflate(Resource.Layout.Style_AddPoll, parent, false);
                var vh = new AddPollAdapterViewHolder(itemView, OnClick, CloseClickListener);
                return vh;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return null;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                Position = position;
                if (viewHolder is AddPollAdapterViewHolder holder)
                {
                    var itemcount = Position + 1;
                    holder.Number.Text = itemcount.ToString(); 
                    holder.Input.Hint = Activity_Context.GetText(Resource.String.Lbl2_Answer) + " " + itemcount;
                }
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }


        public void Clear()
        {
            try
            {
                AnswersList.Clear();
                NotifyDataSetChanged();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public PollAnswers GetItem(int position)
        {
            return AnswersList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return base.GetItemId(position);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return base.GetItemViewType(position);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return 0;
            }
        }

        private void OnClick(AddPollAdapterClickEventArgs args)
        {
            try
            {
                var item = AnswersList[args.Position];
                item.Answer = args.Text;
                args.Input.RequestFocus();
                ItemClick?.Invoke(this, args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void CloseClickListener(AddPollAdapterClickEventArgs args)
        {
            if (AnswersList.Count > 2)
            {
                var item = AnswersList[args.Position];
                AnswersList.Remove(item);
                NotifyDataSetChanged();
                ItemLongClick?.Invoke(this, args);
            }
            else
            {
                
                Snackbar mySnackbar = Snackbar.Make(args.View,
                    Activity_Context.GetText(Resource.String.Lbl2_PollsLimitTwo), Snackbar.LengthShort);
                mySnackbar.Show();
            }
        }
    }

    public class AddPollAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Number;
        public EditText Input;
        public Button CloseButton;
        public AddPollAdapterViewHolder(View itemView, Action<AddPollAdapterClickEventArgs> clickListener,  Action<AddPollAdapterClickEventArgs> CloseClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;
                var circel = (TextView)MainView.FindViewById(Resource.Id.bgcolor);
                Number = (TextView)MainView.FindViewById(Resource.Id.number);
                Input = (EditText)MainView.FindViewById(Resource.Id.text_input);
                CloseButton = (Button)MainView.FindViewById(Resource.Id.Close);

                Typeface font = Typeface.CreateFromAsset(Application.Context.Resources.Assets, "ionicons.ttf");
                CloseButton.SetTypeface(font, TypefaceStyle.Normal);
                CloseButton.Text = IonIcons_Fonts.CloseRound;
                Input.AfterTextChanged+= (sender, e) => clickListener(new AddPollAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Text= Input.Text , Input = Input });
                IMethods.Set_TextViewIcon("1", circel, IonIcons_Fonts.Record);
                CloseButton.Click += (sender, e) => CloseClickListener(new AddPollAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Text = Input.Text });
                //Create an Event
                //itemView.Click += (sender, e) => clickListener(new AddPollAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
           
        }



        #region Variables Basic

        public View MainView { get; }
        public event EventHandler<int> ItemClick;


        public ImageViewAsync Image { get; }

        #endregion
    }

    public class AddPollAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public string Text { get; set; }
        public EditText Input { get; set; }
    }
}