"use client";

import { FormEvent, useState } from "react";

type Status = "idle" | "submitting" | "success" | "error";

export function SignalSignup() {
  const [status, setStatus] = useState<Status>("idle");

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const form = event.currentTarget;
    if (!form.reportValidity()) return;
    setStatus("submitting");

    try {
      const response = await fetch("/signal-form.html", {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: new URLSearchParams(new FormData(form) as unknown as Record<string, string>).toString(),
      });
      if (!response.ok) throw new Error("Transmission rejected");
      form.reset();
      setStatus("success");
    } catch {
      setStatus("error");
    }
  }

  if (status === "success") {
    return (
      <div className="signal-confirmation" role="status" aria-live="polite">
        <span className="signal-confirmation-glyph" aria-hidden="true">◈</span>
        <p>TRANSMISSION RECEIVED</p>
        <h3>Welcome to the Blaze Signal.</h3>
        <blockquote>“Curiosity is the first door. Purpose determines what opens after it.”</blockquote>
        <small>ARCHIVE ACCESS: OBSERVER / SYNCHRONIZATION: PENDING</small>
      </div>
    );
  }

  return (
    <>
      <form className="signup" name="blaze-signal" method="POST" data-netlify="true" data-netlify-honeypot="bot-field" onSubmit={handleSubmit}>
        <input type="hidden" name="form-name" value="blaze-signal" />
        <input type="hidden" name="source" value="elijahblaze.com" />
        <p className="form-honeypot" aria-hidden="true"><label>Leave this field empty <input name="bot-field" tabIndex={-1} autoComplete="off" /></label></p>
        <label className="sr-only" htmlFor="signal-email">Email address</label>
        <input id="signal-email" name="email" type="email" inputMode="email" autoComplete="email" placeholder="YOUR EMAIL ADDRESS" required disabled={status === "submitting"} />
        <button type="submit" disabled={status === "submitting"}>{status === "submitting" ? "TRANSMITTING…" : "JOIN THE SIGNAL"} <span>↗</span></button>
      </form>
      {status === "error" && <p className="signal-error" role="alert">The signal could not be transmitted. Please check your connection and try again.</p>}
      <small className="signal-consent">NO NOISE. NO SPOILERS. UNSUBSCRIBE AT ANY TIME. <a href="/privacy">PRIVACY</a>.</small>
    </>
  );
}
