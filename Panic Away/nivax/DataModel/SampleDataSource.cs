using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Panic-Away is a useful, innovative and insightful technique to take charge and eliminate panic. Its clever and empowering short handed phrases enable a person to detach from their anxiety and challenge it full force. Indeed-this can be the First-Aid kit for anxiety. I would definitely recommend it to my patients.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Acupuncture",
                    "Men and women who are prone to getting panic attacks might be able to seek help in acupuncture. Acupuncture has been tested in clinical studies to improve a variety of disorders, and even diseases. Its effect on panic attacks is related to the way your body responds to fear.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nMen and women who are prone to getting panic attacks might be able to seek help in acupuncture. Acupuncture has been tested in clinical studies to improve a variety of disorders, and even diseases. Its effect on panic attacks is related to the way your body responds to fear. As you're in the midst of panic, your heart might be pounding, you may feel short of breath, you might like you are choking -- you might start sweating, trembling, losing control, or even feeling like you're dying. Many people who are panicking feel like they need to flee the situation, but they are not able to do so. There are incredible number of causes for these panic attacks as well. For some people it is genetic. For others it is related to things going on in their lives at the time. For still others it is related to things like medication or diseases. Diet is now thought of to be a cause of panic attacks for some people as well -- especially candida over-growth or toxic metals.\n\nWhen you begin acupuncture, you'll probably start to discuss your case with your acupuncturist. From there, they can assess where there are imbalances in your body. They may find that you have a Qi (chi) deficiency. Anxiety Qi deficiency is usually found in the kidney or spleen meridians. Your acupuncturist will diagnosis and determine where to treat you. They will use needles in various points on your body. Depending on where these points are, they may release endorphins and balance your hormones. There are even certain relaxation points that can reduce your anxiety and make you feel better all around. Many acupuncturists will also evaluate your diet and exercise patterns, and make suggestions for change. Basically, it is all about balancing your entire body so you can start to feel free from panic attacks. They might prescribe herbs, exercise regimes, or dietary changes depending on the strength of the acupuncturist's program. Even if you've never considered acupuncture for panic attacks before, it is certainly a great holistic method try. Many acupuncturists will give you a free consultation so you can determine if it is right for you. Using acupuncture along with other holistic methods can end your panic attacks for good. \n\nNo matter which methods you try, it's important that you do something as soon as possible. There is no reason to live your life getting panic attacks all the time. Take control and change your life for the better!",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Acupuncture", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Panic Away" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "End Your Panic",
                     "Many people who get panic attacks feel like their life is basically over. They feel out of control and possibly even like they are dying. If that is how you feel, you might seriously doubt there is any way you can end these panic attacks.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nMany people who get panic attacks feel like their life is basically over. They feel out of control and possibly even like they are dying. If that is how you feel, you might seriously doubt there is any way you can end these panic attacks. Fortunately, if you have the right methods, you CAN eliminate the panic and live your life again. Put your doubt aside right now, and grasp onto this hope The first thing you need to do is take stock of your symptoms. People suffer from different symptoms, and your treatment plan might differ depending on the ones who you are dealing with. If you are seeking the advice of a doctor, you should tell your doctor what symptoms you are experiencing so they can help you end your panic attacks. After you have evaluated your symptoms, you need to do some research. Research what works for other people by joining forums that discuss panic attacks. You definitely don't want to focus on negative threads -- focus on positive threads where people have found their answer. There are tons of people who have successfully banished their panic for good and it can be motivating for you to read their stories. It's also a good idea to read books and e-books written by people who have been in your situation. There are many great techniques out there that have worked for thousands of people. By reading about these methods, you can further perfect your own personalized plan to end your panic attacks. Some of these techniques are even downloadable, which is important when you need this information as soon as possible.\n\nAnother thing you might want to do is join a support group. Knowing that there are other people going through the same thing can actually help you get through it all. You feel like you're not alone and that you're not going crazy -- this is a true disorder that you can end with some effort.\n\nAt this point, you probably have a good grasp on what your symptoms are as well as different methods you can try to eliminate your panic attacks. Not all the methods will work for you, so you need to read through them and find the ones that will! After you've done that, it's time to create a plan for getting rid of your panic attacks. Take note of what you can do in the short term as well as the long-term so you can start to feel better.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "End Your Panic", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Common Symptoms",
                     "While many people get panic attacks regularly, the symptoms do not present themselves the same in every individual. If you want to treat your panic attacks, it's important to take stock of your own symptoms so you can eliminate them for good.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nWhile many people get panic attacks regularly, the symptoms do not present themselves the same in every individual. If you want to treat your panic attacks, it's important to take stock of your own symptoms so you can eliminate them for good. One very common problem panic attack sufferers face is feeling like they are not able to breathe. This can lead to a variety of other symptoms, such as feeling dizzy or like you are going to pass out. That's why one of the things you can do when you get a panic attack is breathe in and out through your nose, slowly and rhythmically. Even though this can be helpful, the feeling that you are being smothered, or are choking, is one of the most unfortunate symptoms of panic attacks. In addition to breathing problems, many people experience heart problems. Your heart might start palpitating, pounding, or beating very fast. These symptoms cause some people to believe they're having a heart attack -- especially the first time the panic attack occurs. While no long-term effects will come from these feelings, it does not make them any less devastating in the moment.\n\nYour body might also start to tremble. This is a natural response bodies have when you are fearful or anxious. In times of panic attack, this trembling and nervousness increases a lot. This kind of response can make you even more fearful, and you might start to feel out of control. Speaking of feeling out of control, you might start to get the feeling that you are detached from yourself. These detached feelings cause an odd sense of self. Even if you know deep down that your feelings are irrational, it does nothing to help since you feel so detached. This can cause you to start feeling like you're going crazy or are completely losing your grip on reality. One of the biggest concerns with feeling this way is not being able to take control of your panic attacks. It's like an endless loop -- the more panic attacks you have, and the more severe they get, the more likely you are to have them again. \n\nSince these symptoms are so severe, and encompass both your physiology and psychology, you might start to feel like you are dying or fear dying. This is scary and very difficult to deal with if you do not start to seek treatment and help as soon as possible.\n\nClearly, dealing with the symptoms of panic attacks is no way to live your life. It's time to take action today and seek the help you need to get rid of panic attacks for good.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Common Symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Common Treatments",
                     "Since so many people suffer from panic attacks, doctors and specialists have spent a lot of time developing treatments for panic attack sufferers. Some of these techniques are easy to put into action, and others are quite difficult and life-changing. For example, certain medications may have other side effects that are less desirable than having panic attacks! That's why it's best to evaluate all your options and see what's best for you.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nSince so many people suffer from panic attacks, doctors and specialists have spent a lot of time developing treatments for panic attack sufferers. Some of these techniques are easy to put into action, and others are quite difficult and life-changing. For example, certain medications may have other side effects that are less desirable than having panic attacks! That's why it's best to evaluate all your options and see what's best for you. One of the simplest panic attack treatments is breathing into a paper bag. This can help you focus and get your breathing to return back to normal. Even though this treatment seems simple enough, there is some controversy surrounding it. That's because it can increase the carbon dioxide levels in your system, which can make your panic attacks worse. Instead of breathing into a paper bag, you might consider using certain breathing techniques. Breathing in and out very slowly through your nose can help to balance your body and decrease the responses of fear.\n\nIn addition to breathing techniques, you can also rely on certain medications to help you feel better. Common medications are benzodiazepine drugs. These can help to stop your panic attacks, and help you live a more normal life. Unfortunately, developing a dependence to these kinds of drugs is far too common. There are other side effects that can come from these medications that you'll definitely want to look into before getting started. Some doctors may put you on an antidepressant instead, but these come with their own set of problems. \n\nWhile some rely on medications, others want to take a more natural, or holistic approach. We've already discussed breathing techniques, but there are other things you can do as well. One of the best approaches is repeating coping statements to yourself. For example, you can remind yourself that no one has ever died from having a panic attack -- that the fear is unrealistic. Visualize the absolute worst thing that can happen in this moment in time, and realize that you can handle it after all.\n\nOne of the most powerful things you can realize is that even though it is called a panic attack, it is not something you need to fight. Instead, you can just observe your panic attack and actually welcome in the symptoms. This may sound counterintuitive; but it can absolutely work for you. Instead of getting stuck in a cycle of having panic attacks, you are suddenly not so fearful anymore.\n\nThese are fairly simplistic descriptions of some complicated treatment options that are available to you. It's in your best interest to learn more today so you can devise a treatment plan that will help you end your panic attacks for good.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Cpmmon Treatments", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Steps to End Panic",
                     "When you feel a panic attack starting, the first thing you need to do is acknowledge its presence. This will allow your mind to start taking control instead of letting the fear response take over completely. By acknowledging its presence, you are asserting some power -- something that many panic attack sufferers have difficulty doing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIt's best to education yourself on successful strategies to end panic attacks. Here are 4 steps you can use to end your panic attacks as soon as you feel one coming on.  When you feel a panic attack starting, the first thing you need to do is acknowledge its presence. This will allow your mind to start taking control instead of letting the fear response take over completely. By acknowledging its presence, you are asserting some power -- something that many panic attack sufferers have difficulty doing. After you have acknowledged the presence of your panic attack, you need to relax. This might seem out of place at this point in time, but it will be a lot easier to do after you have acknowledged your panic attack. At the same time you are relaxing, you need to envision the absolute worst thing that could happen as your panic attack is occurring. Think of the thing you are fearful of and what the worst consequence could be.You will probably find that the worst consequence is something you CAN handle. Some people's worst fear will be death. If that it is the consequence you fear the most, you need to switch to using some coping statements instead. You'll need to remind yourself strongly that no one has ever died from a panic attack. There is absolutely no reason to think that you will die from a panic attack. By facing your worst fear, and using coping statements, you will be able to stop your negative thinking and start feeling a sense of peacefulness where the fear used to be./n/nThis will help you to realize that you are a strong person who can overcome these panic attacks. Accept how you feel and stop trying to win the battle against panic attacks. Thinking of this as a battle immediately puts your defenses up -- which is not a good thing when you're having difficulty coping. Instead, acknowledge your panic attack, see that it is coming on, think of the worst thing that can happen, use your coping statements, and you should be able to more easily get over your panic attacks.\n\nKeep in mind that this exact plan will not work for everyone. However, it will for many sufferers. Put it into practice and even write down the steps so you can pull them out whenever you feel the panic coming on. Soon enough, you may naturally be able to cope, and you'll have a lot less difficulty with panic attacks.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Steps to End Panic", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Panic Attack Plan",
                     "As someone who suffers from panic attacks, the chances are good that you have read about many different treatment options for how to get rid of both the symptoms and causes of panic attacks. It can be overwhelming to read all of this information because there are so many different things to try! That's why it's so important to come up with a panic attack plan that is tailored to fit your lifestyle and your specific problem.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAs someone who suffers from panic attacks, the chances are good that you have read about many different treatment options for how to get rid of both the symptoms and causes of panic attacks. It can be overwhelming to read all of this information because there are so many different things to try! That's why it's so important to come up with a panic attack plan that is tailored to fit your lifestyle and your specific problem. Of course, you'll want to work with a professional, or read a professional guide to ensure that you are coming about this the right way. Be sure to carefully choose who you decide to work with, as some doctors will just put you on harmful medications without a second thought. You need to be in control of your own health, so educate yourself as much as possible so you can communicate effectively with professionals.\n\nAfter you have found someone to work with, it's time to evaluate all of your options. Some people have such severe panic attacks that they want to use herbs or medications right away. Some of these can help take away your symptoms in the short term, so you'll need to find a more long-term solution at the same time. Others take a while to work (usually herbs), so you'll need to find a solution in the short term as well. Evaluating each and every herb or medication will be invaluable on your journey to ending panic attacks.\n\nAlong with herbs and medications, there are many breathing techniques you may want to add to your own plan. This can be done when you start to feel a panic attack coming on, or as a coping method to help you get through it. If you feel like this'll be beneficial for you, put it in your plan and follow through with it until you find the exact breathing technique that works for you. In addition to visiting with your doctor or holistic practitioner, you may want to work with an acupuncturist or other specialist as well. By using a combination of treatments, your panic attack plan may be more successful. That's because things like acupuncture help to balance your body and you can decrease your stress and anxiety levels. \n\nBasically, you want to go through all the options you have available to you. Then, write an outline of your panic attack plan. This includes what you're going to do in the short term as well as the long term to get rid of your panic attacks forever. It's a good idea to keep your short term plan (what you do when a panic attack hits) with you at all times so you can pull it out in time for your panic attack. Doing this will help you successfully end your panic attacks forever.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Panic Attack Plan", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Taking Control Health",
                     "Taking Control of Your Health to End Panic Attacks Even though there are many treatment options for panic attacks, many sufferers are finding that changing their diet and becoming healthier is the route to feeling better.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTaking Control of Your Health to End Panic Attacks Even though there are many treatment options for panic attacks, many sufferers are finding that changing their diet and becoming healthier is the route to feeling better. This works best for people who have found that eating certain foods trigger their panic attacks. For many, this includes very sugary foods that have no nutritional value. In addition to certain food triggers, some people have panic attacks because they are deficient in certain vitamins. Vitamins B12 and niacin are helpful vitamins that balance your body. When these are absent, you may become more prone to having panic attacks. Deficiencies in vitamins and minerals can be devastating in more ways than one. The first thing you should do is take stock of what you eat every day. The most helpful way to do this is to keep a food diary for a period of a few days, or even a week. Write down everything you consume -- you'll probably be surprised about what you have eaten by the end of the day! A piece of chocolate here, a few chips there, and another piece of chocolate here -- it all adds up! You might be wondering why everyone who has a poor diet isn't a sufferer of panic attacks. Scientists think this is because there are certain people who are more genetically predisposed to getting panic attacks. If you have this genetic predisposition, eating unhealthily can definitely lead to panic attacks for you. \n\nMake sure you are getting enough of the B group of vitamins as one of your first steps. This is easier to do than cutting out foods, because you can simply take a multivitamin, or singular B vitamins depending on your deficiencies. You can visit a doctor or naturopath to make sure are taking these vitamins is the right step for you. \n\nAfter you have added in the proper nutritional elements, you'll want to consider adding more vegetables and fruits into your diet. Follow the food pyramid and make sure your diet contains plenty of vegetables. A good rule of thumb is to make sure that your plate looks very colorful. This signals different nutrients and vitamins so you can rest assured you're getting a well-balanced diet. \n\nWhile adding in fruits, vegetables, and vitamins is easy enough, it is more difficult to take things away from your diet that you're used to eating. For many people, these include sugary and fatty foods. It's a good idea to cut out these foods one at a time to make it easier for you to handle. Keep at it for a period of 30 days, after which it will become a habit for you to avoid the foods that can trigger your panic attacks, and add the ones that will help you.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Taking Control Health", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Taking Control Naturally",
                     "Instead of relying on medication to end your panic attacks, you should consider holistic methods instead. These can generally be used without side effects and are quite effective. Instead of masking what is going on, you can get to the root of things and truly end your panic attacks for good.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nInstead of relying on medication to end your panic attacks, you should consider holistic methods instead. These can generally be used without side effects and are quite effective. Instead of masking what is going on, you can get to the root of things and truly end your panic attacks for good! Before you understand why you should stick to holistic methods, it's important to understand why you may want to steer clear of medication. First of all, the medication can get quite expensive over time, and generally offers only short-term results. In addition to that, you may become addicted to the medication, or experience undesired side effects. It's time to focus on natural methods! One of the most commonly used holistic techniques includes breathing exercises. While breathing into a paper bag is a commonly cited method, you may want to find a different breathing technique, as you'll be breathing in carbon dioxide that could further your symptoms. Instead, practice breathing in and out very slowly through your nose. You can find breathing CDs that will help you take cleansing breaths and inhale air that will nurture your body.\n\nMeditation goes very well alongside breathing exercises. Meditation can come in many different forms, and be as formal or informal as you want it to be. Some people simply sit down and breathe in and slowly as they think good thoughts. Others do chanting exercises and hum as their form of meditation. No matter what you choose to do, meditation can be part of your plan to end your panic attacks. Exercise has also been known to help decrease the instance of panic attacks. That's because exercise can actually help you feel good, and increase the strength of your body and your mind. If you start to feel anxious, get out and start exercising and you will probably feel better. For severe cases, however, this may not be enough for you. Some people turn to treatments like acupuncture or rock therapies to help ease panic attacks. There are certain points on the body that can induce a feeling of relaxation. By attending these sessions regularly, you may start to see a decrease in the number of panic attacks you have.\n\nEven though the techniques above are very useful, they may not be enough for the most severe cases. You can find holistic treatments of herbs to decrease your panic attacks. Be sure to consult with a naturopath or herbalist before treating yourself, as herbs can be strong and contradict medications or other herbs you are taking. \n\nTreating your panic attacks should be a priority in your life right now. That doesn't mean you need to rely on dangerous medications to do so! Find the natural panic attack treatment that will work for you today so you can start to feel better as soon as possible.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Taking Control Naturally", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Using Visualization",
                     "Visualization has long been used for a variety of physical and psychological problems for thousands of years. The mind is a powerful thing -- especially the subconscious mind. By visualizing, you'll be tapping into the power your mind has to heal. Even if you've never tried this before, you'll definitely want to, as it can help you get rid of your panic attacks.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nVisualization has long been used for a variety of physical and psychological problems for thousands of years. The mind is a powerful thing -- especially the subconscious mind. By visualizing, you'll be tapping into the power your mind has to heal. Even if you've never tried this before, you'll definitely want to, as it can help you get rid of your panic attacks. When you feel a panic attack coming on, you can close your eyes and start to visualize whatever helps you the most. For some people, visualizing a humorous experience is the best medicine. Visualize having uncontrolled laughter that takes over your whole body and makes it shake with pleasure. This is a good method because laughter is healing and will counteract the feelings of panic or fear you've been getting from the panic attacks. You can also visualize yourself in a peaceful and calm place. You can literally feel like you are removed from the fearful situation and are somewhere peaceful -- like the beach or staring at a snow capped mountain. Imagine the setting in as vivid detail as possible. \n\nIt might be difficult for first time people to visualize effectively. For that reason, you might consider getting some CDs or a video to help you with this process. There are several out there on the market that will be immensely helpful for you. Some therapists even specialize in visualization, so you can make an appointment with them and they can teach you some techniques.\n\nNo matter what you do, definitely give visualization a try. If you think about it, when you're in the midst of a panic attack you start to have irrational fears. Pictures often flash through your mind. This causes even more symptoms -- your heart might palpitate, and you may even feel like you are dying. It's all because of what is in your mind and what you are picturing. By replacing these scary images with something peaceful, funny, pleasant -- whatever -- you can more easily get over your panic attacks and start to realize that your fears are irrational. You'll feel like all is right in the world after all.\n\nAs was already mentioned, you might even feel a little bit silly doing this at first, and it can be difficult for you. Practice truly does make perfect! The first few times you do this, you might find the method is unsuccessful. However, if you stick with it and allow your conscious and subconscious mind to think of peaceful things, you can beat your panic attacks.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Using Visualization", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Panic Away" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "What is a Panic Attack",
                     "If you're dealing with panic attacks, the chances are good you feel like the world is closing in on you much of the time. Some people experience these attacks infrequently, while others deal with them on a daily basis. No matter how often they occur, it can be devastating when they do. That's why it's so important to seek treatment as soon as possible.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you're dealing with panic attacks, the chances are good you feel like the world is closing in on you much of the time. Some people experience these attacks infrequently, while others deal with them on a daily basis. No matter how often they occur, it can be devastating when they do. That's why it's so important to seek treatment as soon as possible. Panic attacks generally come on suddenly in the form of intense anxiety. You become fearful and feel very unwell and uncomfortable. There are many different symptoms that occur as a result of having a panic attack -- and these depend on both your physiology and mindset. When you're having a panic attack, your body releases adrenaline because you mistakenly believe you are in harm's way. You probably feel very scared and upset. Unfortunately, these attacks can last anywhere from under a minute to well over a half hour.\n\nSome people who are having a panic attack mistakenly believe they need to go to the hospital. That's because it can feel like you are having a heart attack or other form of health problem. The silver lining here is that a panic attack cannot truly hurt you. The downside is obviously that you FEEL like your body is under attack and you're in danger.\n\nThere is no singular reason why panic attacks occur. Sometimes it is passed along within families. If someone else in your family gets panic attacks, the chances of you getting them increase. There are also illnesses that can cause the instance of panic attacks to increase in certain groups of people. An example of this is post traumatic stress disorder. Even hyperthyroidism can cause panic attacks! In addition to these biological reasons, panic attacks can occur as a result of things that have happened to you in the past. Perhaps something scared you in a certain situation years ago, and now you are fearful in similar situations. Panic attacks can also occur as a side effect of certain medications. Ritalin is one drug that can lead to panic attacks in some people. The SSRI group of antidepressants have been known to increase anxiety.\n\nUnderstanding what may be causing your panic attacks is one of the first steps you can take in dealing with them. There are no two ways about it -- panic attacks can be devastating and interfere with nearly every aspect of your life. By taking action today, you can start to lead a normal life again.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "What is a Panic Attack", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Panic Away" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
