import assert from "node:assert/strict";
import { readFile } from "node:fs/promises";
import test from "node:test";

const root = new URL("../", import.meta.url);

async function source(path, encoding = "utf8") {
  return readFile(new URL(path, root), encoding);
}

test("builds the cinematic public homepage", async () => {
  const html = await source(".next/server/app/index.html");

  assert.match(html, /<title>Elijah Blaze — Beneath Eden<\/title>/);
  assert.match(html, /Some dreams are not your own\./);
  assert.match(html, /elijah-blaze-avatar\.png/);
  assert.match(html, /alt="Elijah Blaze in blue-and-gold living guardian armor, crowned by flame"/);
  assert.match(html, /href="#game">Game<\/a>/);
  assert.match(html, /href="#books">Beneath Eden<\/a>/);
  assert.match(html, /href="#codex">Codex<\/a>/);
  assert.match(html, /href="#studio">Studio<\/a>/);
  assert.match(html, /href="#signal">Signal<\/a>/);
  assert.match(html, /The first door is opening\./);
});

test("preserves the Netlify Blaze Signal contract", async () => {
  const [component, staticForm] = await Promise.all([
    source("app/SignalSignup.tsx"),
    source("public/signal-form.html"),
  ]);

  for (const file of [component, staticForm]) {
    assert.match(file, /blaze-signal/);
    assert.match(file, /name=["']email["']/);
  }

  assert.match(component, /fetch\("\/signal-form\.html"/);
  assert.match(component, /name="source" value="elijahblaze\.com"/);
  assert.match(component, /data-netlify="true"/);
  assert.match(staticForm, /data-netlify="true"/);
});

test("ships the canonical guardian and accessible motion controls", async () => {
  const [avatar, css, agents, netlify] = await Promise.all([
    source("public/assets/elijah-blaze-avatar.png", null),
    source("app/globals.css"),
    source("AGENTS.md"),
    source("netlify.toml"),
  ]);

  assert.deepEqual([...avatar.subarray(0, 8)], [137, 80, 78, 71, 13, 10, 26, 10]);
  assert.match(agents, /canonical project avatar is `assets\/elijah-blaze-avatar\.png`/i);
  assert.match(css, /:focus-visible/);
  assert.match(css, /prefers-reduced-motion:\s*reduce/);
  assert.match(netlify, /@netlify\/plugin-nextjs/);
});
