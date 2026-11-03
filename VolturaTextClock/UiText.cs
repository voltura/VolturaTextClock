using System.Collections.Generic;
using System.Globalization;

namespace VolturaTextClock;

internal static class UiText
{
    internal static string Language { get; set; } = "System";
    internal static bool Swedish => Language == "Swedish" || (Language == "System" && CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "sv");
    private static readonly Dictionary<string, (string En, string Sv)> Text = new()
    {
        ["Settings"] = ("Clock settings", "Klockinställningar"),
        ["General"] = ("General", "Allmänt"),
        ["Appearance"] = ("Appearance", "Utseende"),
        ["UpdatesAbout"] = ("Updates & About", "Uppdateringar och Om"),
        ["Save"] = ("Save", "Spara"), ["Cancel"] = ("Cancel", "Avbryt"),
        ["Startup"] = ("Start with Windows", "Starta med Windows"),
        ["Minimized"] = ("Start minimized in the taskbar", "Starta minimerad i aktivitetsfältet"),
        ["OnTop"] = ("Keep the clock on top", "Visa klockan över andra fönster"),
        ["Automatic"] = ("Check for updates when the clock starts", "Sök efter uppdateringar vid start"),
        ["Interface"] = ("Interface language", "Gränssnittsspråk"),
        ["ClockLanguage"] = ("Clock language", "Klockans språk"),
        ["System"] = ("Follow Windows", "Följ Windows"),
        ["English"] = ("English", "Engelska"), ["Swedish"] = ("Swedish", "Svenska"),
        ["ActiveFont"] = ("Active text font", "Aktiv texts teckensnitt"),
        ["InactiveFont"] = ("Inactive text font", "Inaktiv texts teckensnitt"),
        ["TextColors"] = ("Text colors", "Textfärger"),
        ["Active"] = ("Active", "Aktiv"), ["Inactive"] = ("Inactive", "Inaktiv"),
        ["Glow"] = ("Glow", "Glöd"), ["Flicker"] = ("Flicker", "Flimmer"),
        ["Effects"] = ("Text effects", "Texteffekter"),
        ["Background"] = ("Background", "Bakgrund"),
        ["Original"] = ("Original", "Original"), ["Brushed"] = ("Brushed metal", "Borstad metall"),
        ["Rust"] = ("Rust", "Rost"), ["None"] = ("Plain", "Enfärgad"), ["Custom"] = ("Custom image", "Egen bild"),
        ["ChooseImage"] = ("Choose image…", "Välj bild…"),
        ["NoImage"] = ("No custom image selected", "Ingen egen bild vald"),
        ["Preview"] = ("Live preview", "Förhandsvisning"),
        ["LanguageHint"] = ("The clock and interface can use different languages. Other Windows languages use English.", "Klockan och gränssnittet kan ha olika språk. Andra Windows-språk använder engelska."),
        ["RestartHint"] = ("Interface language changes when you save.", "Gränssnittets språk ändras när du sparar."),
        ["Check"] = ("Check for updates", "Sök efter uppdateringar"),
        ["Download"] = ("Download update", "Hämta uppdatering"),
        ["Install"] = ("Install and restart", "Installera och starta om"),
        ["Website"] = ("Product website", "Produktwebbplats"), ["Source"] = ("Source code", "Källkod"),
        ["Report"] = ("Report an issue", "Rapportera ett problem"), ["License"] = ("GPL-3.0 license", "GPL-3.0-licens"),
        ["Privacy"] = ("Your settings stay on this PC. Optional update checks contact GitHub.", "Dina inställningar sparas på datorn. Valfria uppdateringssökningar kontaktar GitHub."),
        ["About"] = ("Time, in words.", "Tid, med ord."),
        ["CloseQuestion"] = ("Close Voltura TextClock?", "Vill du stänga Voltura TextClock?"),
        ["Close"] = ("Close", "Stäng"), ["Options"] = ("Show clock controls", "Visa klockans knappar"),
        ["Pin"] = ("Keep on top", "Visa över andra fönster"), ["Minimize"] = ("Minimize", "Minimera"),
        ["BadImage"] = ("This image could not be opened. Choose a PNG, JPEG or bitmap image.", "Bilden kunde inte öppnas. Välj en PNG-, JPEG- eller bitmappsbild."),
        ["SaveFailed"] = ("Settings could not be saved. Check your user folder permissions and try again.", "Inställningarna kunde inte sparas. Kontrollera användarmappens behörigheter och försök igen."),
        ["StartupFailed"] = ("The clock could not start. Check your user folder permissions and try again.", "Klockan kunde inte starta. Kontrollera användarmappens behörigheter och försök igen."),
        ["Idle"] = ("Check for a newer release when you need it.", "Sök efter en ny utgåva när du behöver det."),
        ["ManualUpdates"] = ("Portable copy: download a new release from GitHub to update.", "Portabel version: hämta en ny utgåva från GitHub för att uppdatera."),
        ["Checking"] = ("Checking for updates…", "Söker efter uppdateringar…"),
        ["Downloading"] = ("Downloading and verifying the update…", "Hämtar och verifierar uppdateringen…"),
        ["Available"] = ("A verified update is available to download.", "En verifierad uppdatering finns att hämta."),
        ["Cancelled"] = ("Update cancelled. You can try again.", "Uppdateringen avbröts. Du kan försöka igen."),
        ["Ready"] = ("The verified update is ready to install.", "Den verifierade uppdateringen är klar att installera."),
        ["Current"] = ("You have the latest release.", "Du har den senaste utgåvan."),
        ["Installing"] = ("Starting setup…", "Startar installationen…"),
        ["UpdateCheckFailed"] = ("Updates could not be checked. Check your connection and try again later.", "Det gick inte att söka efter uppdateringar. Kontrollera anslutningen och försök igen senare."),
        ["UpdateDownloadFailed"] = ("The update could not be downloaded. Try again later.", "Uppdateringen kunde inte hämtas. Försök igen senare."),
        ["UpdateVerificationFailed"] = ("Update verification failed. Nothing was installed.", "Uppdateringen kunde inte verifieras. Inget installerades."),
        ["UpdateInstallFailed"] = ("Setup could not start. Try again or download the release from GitHub.", "Installationen kunde inte starta. Försök igen eller hämta utgåvan från GitHub."),
        ["UpdatePrompt"] = ("A newer version is available. Open settings to update?", "En ny version finns tillgänglig. Vill du öppna inställningarna för att uppdatera?"),
        ["Bold"] = ("bold", "fet"), ["Italic"] = ("italic", "kursiv")
    };
    internal static string Get(string key) => Text.TryGetValue(key, out var text)
        ? (Swedish ? text.Sv : text.En)
        : (Swedish ? "Uppdateringen kunde inte verifieras." : "The update could not be verified.");
}
