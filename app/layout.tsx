import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  metadataBase: new URL("https://elijahblaze.com"),
  title: "Elijah Blaze — Beneath Eden",
  description: "Enter the world of Elijah Blaze: Beneath Eden, the Hollow Court game prototype, and the living Blaze Codex.",
  applicationName: "Elijah Blaze",
  authors: [{ name: "Elijah Blaze Studios", url: "https://elijahblaze.com" }],
  creator: "Elijah Blaze Studios",
  keywords: ["Elijah Blaze", "Beneath Eden", "Elijah Blue", "AZEL", "Hollow Court", "fantasy", "science fantasy"],
  alternates: { canonical: "/" },
  robots: { index: true, follow: true },
  icons: { icon: "/favicon.svg", shortcut: "/favicon.svg" },
  openGraph: { type: "website", url: "/", siteName: "Elijah Blaze", title: "Elijah Blaze — Beneath Eden", description: "Some dreams are not your own.", images: [{ url: "/og.png", width: 1536, height: 1024, alt: "Elijah Blaze in blue-and-gold living guardian armor" }] },
  twitter: { card: "summary_large_image", title: "Elijah Blaze — Beneath Eden", description: "Some dreams are not your own.", images: ["/og.png"] },
};

export default function RootLayout({ children }: Readonly<{children: React.ReactNode}>) {
  return <html lang="en"><body>{children}</body></html>;
}
