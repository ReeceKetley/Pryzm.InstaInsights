﻿using System;

public static class Emojis
{
    public static string[] Items = new[] { "⌚", "⌛", "⏩", "⏪", "⏫", "⏬", "⏰", "⏳", "Ⓜ", "▪", "▫", "▶", "◀", "◻", "◼", "◽", "◾", "☀", "☁", "☎", "☑", "☔", "☕", "☝", "☺", "♈", "♉", "♊", "♋", "♌", "♍", "♎", "♏", "♐", "♑", "♒", "♓", "♠", "♣", "♥", "♦", "♨", "♻", "♿", "⚓", "⚠", "⚡", "⚪", "⚫", "⚽", "⚾", "⛄", "⛅", "⛎", "⛔", "⛪", "⛲", "⛳", "⛵", "⛺", "⛽", "✂", "✅", "✈", "✉", "✊", "✋", "✌", "✏", "✒", "✔", "✖", "✨", "✳", "✴", "❄", "❇", "❌", "❎", "❓", "❔", "❕", "❗", "❤", "➕", "➖", "➗", "➡", "➰", "⤴", "⤵", "⬅", "⬆", "⬇", "⬛", "⬜", "⭐", "⭕", "〰", "〽", "㊗", "㊙", "🀄", "🃏", "🅰", "🅱", "🅾", "🅿", "🆎", "🆑", "🆒", "🆓", "🆔", "🆕", "🆖", "🆗", "🆘", "🆙", "🆚", "🇨 🇳", "🇩 🇪", "🇪 🇸", "🇫 🇷", "🇬 🇧", "🇮 🇹", "🇯 🇵", "🇰 🇷", "🇷 🇺", "🇺 🇸", "🈁", "🈂", "🈚", "🈯", "🈲", "🈳", "🈴", "🈵", "🈶", "🈷", "🈸", "🈹", "🈺", "🉐", "🉑", "🌀", "🌁", "🌂", "🌃", "🌄", "🌅", "🌆", "🌇", "🌈", "🌉", "🌊", "🌋", "🌌", "🌏", "🌑", "🌓", "🌔", "🌕", "🌙", "🌛", "🌟", "🌠", "🌰", "🌱", "🌴", "🌵", "🌷", "🌸", "🌹", "🌺", "🌻", "🌼", "🌽", "🌾", "🌿", "🍀", "🍁", "🍂", "🍃", "🍄", "🍅", "🍆", "🍇", "🍈", "🍉", "🍊", "🍌", "🍍", "🍎", "🍏", "🍑", "🍒", "🍓", "🍔", "🍕", "🍖", "🍗", "🍘", "🍙", "🍚", "🍛", "🍜", "🍝", "🍞", "🍟", "🍠", "🍡", "🍢", "🍣", "🍤", "🍥", "🍦", "🍧", "🍨", "🍩", "🍪", "🍫", "🍬", "🍭", "🍮", "🍯", "🍰", "🍱", "🍲", "🍳", "🍴", "🍵", "🍶", "🍷", "🍸", "🍹", "🍺", "🍻", "🎀", "🎁", "🎂", "🎃", "🎄", "🎅", "🎆", "🎇", "🎈", "🎉", "🎊", "🎋", "🎌", "🎍", "🎎", "🎏", "🎐", "🎑", "🎒", "🎓", "🎠", "🎡", "🎢", "🎣", "🎤", "🎥", "🎦", "🎧", "🎨", "🎩", "🎪", "🎫", "🎬", "🎭", "🎮", "🎯", "🎰", "🎱", "🎲", "🎳", "🎴", "🎵", "🎶", "🎷", "🎸", "🎹", "🎺", "🎻", "🎼", "🎽", "🎾", "🎿", "🏀", "🏁", "🏂", "🏃", "🏄", "🏆", "🏈", "🏊", "🏠", "🏡", "🏢", "🏣", "🏥", "🏦", "🏧", "🏨", "🏩", "🏪", "🏫", "🏬", "🏭", "🏮", "🏯", "🏰", "🐌", "🐍", "🐎", "🐑", "🐒", "🐔", "🐗", "🐘", "🐙", "🐚", "🐛", "🐜", "🐝", "🐞", "🐟", "🐠", "🐡", "🐢", "🐣", "🐤", "🐥", "🐦", "🐧", "🐨", "🐩", "🐫", "🐬", "🐭", "🐮", "🐯", "🐰", "🐱", "🐲", "🐳", "🐴", "🐵", "🐶", "🐷", "🐸", "🐹", "🐺", "🐻", "🐼", "🐽", "🐾", "👀", "👂", "👃", "👄", "👅", "👆", "👇", "👈", "👉", "👊", "👋", "👌", "👍", "👎", "👏", "👐", "👑", "👒", "👓", "👔", "👕", "👖", "👗", "👘", "👙", "👚", "👛", "👜", "👝", "👞", "👟", "👠", "👡", "👢", "👣", "👤", "👦", "👧", "👨", "👩", "👪", "👫", "👮", "👯", "👰", "👱", "👲", "👳", "👴", "👵", "👶", "👷", "👸", "👹", "👺", "👻", "👼", "👽", "👾", "🤖", "👿", "💀", "💁", "💂", "💃", "💄", "💅", "💆", "💇", "💈", "💉", "💊", "💋", "💌", "💍", "💎", "💏", "💐", "💑", "💒", "💓", "💔", "💕", "💖", "💗", "💘", "💙", "💚", "💛", "💜", "💝", "💞", "💟", "💠", "💡", "💢", "💣", "💤", "💥", "💦", "💧", "💨", "💩", "💪", "💫", "💬", "💮", "💯", "💰", "💱", "💲", "💳", "💴", "💵", "💸", "💹", "💺", "💻", "💼", "💽", "💾", "💿", "📀", "📁", "📂", "📃", "📄", "📅", "📆", "📇", "📈", "📉", "📊", "📋", "📌", "📍", "📎", "📏", "📐", "📑", "📒", "📓", "📔", "📕", "📖", "📗", "📘", "📙", "📚", "📛", "📜", "📝", "📞", "📟", "📠", "📡", "📢", "📣", "📤", "📥", "📦", "📧", "📨", "📩", "📪", "📫", "📮", "📰", "📱", "📲", "📳", "📴", "📶", "📷", "📹", "📺", "📻", "📼", "🔃", "🔊", "🔋", "🔌", "🔍", "🔎", "🔏", "🔐", "🔑", "🔒", "🔓", "🔔", "🔖", "🔗", "🔘", "🔙", "🔚", "🔛", "🔜", "🔝", "🔞", "🔟", "🔠", "🔡", "🔢", "🔣", "🔤", "🔥", "🔦", "🔧", "🔨", "🔩", "🔪", "🔫", "🔮", "🔯", "🔰", "🔱", "🔲", "🔳", "🔴", "🔵", "🔶", "🔷", "🔸", "🔹", "🔺", "🔻", "🔼", "🔽", "🕐", "🕑", "🕒", "🕓", "🕔", "🕕", "🕖", "🕗", "🕘", "🕙", "🕚", "🕛", "🗻", "🗼", "🗽", "🗾", "🗿", "😁", "😂", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😌", "😍", "😏", "😒", "😓", "😔", "😖", "😘", "😚", "😜", "😝", "😞", "😠", "😡", "😢", "😣", "😤", "😥", "😨", "😩", "😪", "😫", "😭", "😰", "😱", "😲", "😳", "😵", "😷", "😸", "😹", "😺", "😻", "😼", "😽", "😾", "😿", "🙀", "🙅", "🙆", "🙇", "🙈", "🙉", "🙊", "🙋", "🙌", "🙍", "🙎", "🙏", "🚀", "🚃", "🚄", "🚅", "🚇", "🚉", "🚌", "🚏", "🚑", "🚒", "🚓", "🚕", "🚗", "🚙", "🚚", "🚢", "🚤", "🚥", "🚧", "🚨", "🚩", "🚪", "🚫", "🚬", "🚭", "🚲", "🚶", "🚹", "🚺", "🚻", "🚼", "🚽", "🚾", "🛀", "🚛", "😙", "🍐", "🚴", "🐇", "🕣", "🚋", "🚘", "😑", "😈", "😦", "😶", "🍼", "🚱", "😮", "🌜", "🚯", "😎", "➿", "🌗", "😀", "💶", "🕞", "🔭", "🌐", "📯", "😛", "🕥", "💷", "👬", "🐅", "😧", "🚦", "😕", "🔁", "🚔", "🚊", "🐉", "🌎", "🏉", "🛅", "🔉", "🕡", "🐪", "🚍", "🏇", "🐓", "🚣", "🛃", "🔂", "🌒", "🚞", "🕤", "🚮", "🔄", "🕜", "🐐", "🐖", "😇", "🚳", "🚈", "🐋", "🚆", "🌍", "🚿", "🌖", "🚂", "🐈", "🚜", "💭", "👭", "🌝", "🐁", "🕟", "😟", "🐀", "🐏", "🐕", "😗", "🚁", "🕦", "📵", "🏤", "🐂", "🚠", "😴", "🐄", "🚐", "🕢", "🚡", "🔈", "🔕", "📬", "🚷", "🔬", "🛁", "🚟", "🐊", "🚵", "🌘", "🚝", "🚸", "🕝", "👥", "📭", "🐆", "🌳", "🚖", "🍋", "🔇", "🛄", "🔀", "🌞", "🚎", "🌲", "🛂", "🌚", "🚰", "🔆", "🔅", "🕠", "😯", "😬", "🐃", "😐", "🕧" };
}
