export const metadata = {
  title: "Privacy — Elijah Blaze",
  description: "Privacy information for the Blaze Signal.",
};

export default function PrivacyPage() {
  return (
    <main className="privacy-page">
      <a className="privacy-back" href="/">← RETURN TO THE ARCHIVE</a>
      <p className="kicker">BLAZE SIGNAL / PRIVACY</p>
      <h1>Your signal belongs to you.</h1>
      <p>When you join the Blaze Signal, we collect only the email address you provide and a simple source label identifying ElijahBlaze.com.</p>
      <h2>How it is used</h2>
      <p>Your address is used only for Elijah Blaze development notes, recovered lore, prototype updates, and publication news. It is not sold or shared for unrelated advertising.</p>
      <h2>Leaving the Signal</h2>
      <p>Every email transmission will include an unsubscribe option. You may also request removal by replying to any Signal message.</p>
      <h2>Storage</h2>
      <p>Initial subscriptions are processed and stored through Netlify Forms. Access is limited to the Elijah Blaze Studios site owner and authorized services used to deliver the Signal.</p>
      <p className="privacy-updated">LAST UPDATED / JULY 22, 2026</p>
    </main>
  );
}
