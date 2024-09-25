# ED voicing events

## Purpose:
This application is a handler for the Elite:Dangerous pilot log events. It automatically monitors the latest log file for changes and processes the events in it, according to the configuration file (***config\\config.xml***)

###### What an application can do by responding to events:

1. Play audio files in synchronous and asynchronous modes. Stop playback in asynchronous mode.
2. Speak game text and user-defined phrases.
3. Change the pronunciation voice, its volume and speed.
4. In accordance with the parameters of the event, make one decision or another.
5. To make one decision or another at random.

## Application setup:
1. Download and install voice synthesizers for SAPI (Windows) https://rhvoice.ru/languages/ (russian/english voices). You can also download voices from https://rhvoice.su/voices/
2. Download the latest release from GitHub (Pre-save the folder **%APPDATA%/ELiteVoice**, otherwise you may lose all configuration settings).
3. Launch the program from **Start->Applications->EliteVoice C3S->EliteVoice**. Processing has started (the application can be started/stopped at any time).

## Configuration syntax (%APPDATA%\\EliteVoice\\config\\config.xml):

Element | Attributes                                                                                                                                                | Description
------- |-----------------------------------------------------------------------------------------------------------------------------------------------------------| --------
Configuration |                                                                                                                                                           | Root configuration element
Init |                                                                                                                                                           | Application initialization section
Replace | **match** - regular expression for text replacement<br>**replace** - replacement string<br>**ignorecase**[**true**\|false] - case insensitive replacement | Global replacement of spoken text
Event | **name** - event name or XPath expression                                                                                                                 | The event handling section of the game. All child commands are executed when this event occurs.
Play | **name** - player name<br>**file** - audio file name<br>**volume**[0..100] - volume level<br>**async**[true\|**false**] - asynchronous playback           | Play audio file
Stop | **name** - player name<br>**fade** - decay time in milliseconds                                                                                           | Stop the audio file player. If the **name** parameter is missing, all players are stopped.
TextToSpeech | **voice** - voice acting name<br>**volume**[0..100] - volume level<br>**rate**[-10..10] - speed of pronunciation<br>                                      | Set text pronunciation settings for child elements or globally.
Text | **select** - XPath expression to select spoken text                                                                                                       | Speak text from an event parameter or take it from the element's content.
Pause | **value** - stop time in milliseconds                                                                                                               | Pause command execution for **value** milliseconds.
Randomize |                                                                                                                                                           | Choose one of the child commands randomly.
Block | **priority**[**1**..] - block selection priority, the higher the greater the probability                                                                          | Command block for the Randomize element.
Switch | **select** - XPath expression to select an element                                                                                                          | Selecting a parameter for a selection condition in Case/Default child elements
Case | **match\|imatch** - matches a regular expression<br>**equal\|iequal** - full match<br>**test** - test against an XPath expression             | Tests the condition of the parameter value specified by the Switch element. The **i** prefix on the attribute means that the comparison will be case-insensitive.
Default |                                                                                                                                                           | Default condition in Switch block. Must be the **last** element.
If | **test** - check by XPath expression                                                                                                                    | Checks a condition specified by an XPath expression. Child elements are executed if the condition is true.
ForEach | **select** - XPath expression to select a set of elements                                                                                                  | Select a set of elements specified by an XPath expression.



###### Some explanations:

Unlike the previous version of the application, which used JSON directly from the log file, the current version converts JSON to XML. This gives an advantage in parsing event parameters, additional calculations, traversing the structure, etc. For example: converting the JSON string of the StartJump event:
```json
{
  "timestamp":"2021-09-28T02:12:34Z",
  "event":"StartJump",
  "JumpType":"Hyperspace",
  "StarSystem":"Didio",
  "SystemAddress":3240309573995,
  "StarClass":"G"
}
```
will be converted to XML of the form:
```xml
<root>
    <timestamp>2021-09-28T02:12:34Z</timestamp>
    <event>StartJump</event>
    <JumpType>Hyperspace</JumpType>
    <StarSystem>Didio</StarSystem>
    <SystemAddress>3240309573995</SystemAddress>
    <StarClass>G</StarClass>
</root>
```
As you can see, the current element for processing commands is **root**, so to voice, for example, the name of a star system, you need to use an expression like **"//StarSystem"**. In the examples below, you should pay attention to this.

###### Samples:

Execute a block of commands when the *SupercruiseEntry* event occurs:
```xml
<Event name="SupercruiseEntry">
    ...
</Event>
```

Set globally the voice "Anna" with the fastest pronunciation and half the volume. Say the phrase. Switch to the voice "Irina" and medium speed. Say the phrase. And again with the voice "Anna" say the phrase. The pauses between pronunciations will be 500 milliseconds.
```xml
<TextToSpeech voice="Anna" volume="50" rate="10"/>
<Text>Hello people! This is Anna speaking.</Text>
<Pause value="500"/>
<TextToSpeech voice="Irina" rate="0">
<Text>Irina intervenes in the process</Text>
</TextToSpeech>
<Pause value="500"/>
<Text>And now Anna will speak again.</Text>
```


Randomly pronounce one of three phrases. The probability of pronouncing the first phrase is 20%, the second - 50%, the third - 30%:
```xml
<Randomize>
    <Block priority="2">
        <Text>Первая фраза</Text>
    </Block>
    <Block priority="5">
        <Text>Вторая фраза</Text>
    </Block>
    <Block priority="3">
        <Text>Третья фраза</Text>
    </Block>
</Randomize>
```

Play the sound file at 20% volume. After it is finished, say the phrase.
```xml
<Play file="sound\music.mp3" volume="20"/>
<Text>The music has ended</Text>
```

Start playing the audio file at 50% volume. After 10 seconds, say the phrase and stop playing within 5 seconds.
```xml
<Play name="player1" file="sound\music.mp3" volume="50" async="true"/>
<Pause value="10000"/>
<Text>Now the music will start to end.</Text>
<Stop name="player1" fade="5000"/>
```

Check who the message came from in the game and accordingly choose the value of which parameter to use to pronounce the message.
```xml
<Switch select="/*/Channel">
    <Case imatch="(player|wing)">
        <Text select="../Message"/>
    </Case>
    <Default>
        <Text select="../Message_Localised"/>
    </Default>
</Switch>
```

Initialization section. Globally replace "mom" with "dad". Replace "Euryale" with "Home system Euryale".
```xml
  <Init>
    <Replace match="mom" replace="dad"/>
    <Replace match="Euryale" replace="Home system Euryale"/>
    ...
</Init>
```

Check if the system is inhabited and speak the population and number of factions in the system in the FSDJump event.
```xml
<Event name="FSDJump">
    <Switch select="//Population">
        <Case iequal="0">
            <Text>Uninhabited system</Text>
        </Case>
        <Default>
            <Text>System population:</Text>
            <Text select="./text()"/>
            <Text>peoples</Text>
            <Pause value="500"/>
            <Text>Total fractions: </Text>
            <Text select="count(../Factions)"/>
            <Pause value="500"/>
            <Text>Controlling faction:</Text>
            <Text select="../SystemFaction"/>
        </Default>
    </Switch>
</Event>
```

!!!Important. The program contains a settings file **%APPDATA%\\EliteVoice\\log4net\\log4net.config**, which configures logging of the output of JSON->XML transformations. If you do not need to debug the XML output, simply delete this file or, better yet, replace **DEBUG** with **NONE**.

## Notes:

1.	To understand that RHVoice with your voices has connected, you can look at the debug output to see which voices are connected in the system. Personally, I have these:

	**Found voices:**<br>
	Arina<br>
	Alan<br>
	Aleksandr<br>
	Aleksandr+Alan<br>
	Aleksandr-hq<br>
	Anna<br>
	Microsoft Zira Desktop - English (United States)<br>
	Artemiy<br>
	Bdl<br>
	Clb<br>
	Elena<br>
	Evgeniy-Rus<br>
	Irina<br>
	Mikhail<br>
	Pavel<br>
	Slt<br>
	Tatiana<br>
	Tatiana+Clb<br>
	Victoria<br>
	Yuriy<br>

2.  In the debug output you can see which events are processed and which are not. And accordingly write/change their handlers.

3.	Of course there are errors in the program. Report errors to me on [github](https://github.com/hapsys/elite-voice/issues)
