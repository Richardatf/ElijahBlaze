import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Elijah Blaze — Beneath Eden",
  description: "The public development hub for Beneath Eden, the Unity game, and the Blaze Codex.",
  icons: { icon: "/favicon.svg", shortcut: "/favicon.svg" },
  openGraph: { title: "Elijah Blaze — Beneath Eden", description: "Some dreams are not your own.", images: [{ url: "/og.png", width: 1536, height: 1024 }] },
  twitter: { card: "summary_large_image", title: "Elijah Blaze — Beneath Eden", description: "Some dreams are not your own.", images: ["/og.png"] },
};

export default function RootLayout({ children }: Readonly<{children: React.ReactNode}>) {
  return <html lang="en"><body>{children}</body></html>;
}
