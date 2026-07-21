"use client";

import { useEffect, useState } from "react";

type Entry = {
  number: string;
  title: string;
  classification: string;
  glyph: string;
  summary: string;
  label: string;
  passage: string;
  details: Array<[string, string]>;
  references: string[];
  sealed: string;
};

const entries: Entry[] = [
  {
    number: "01",
    title: "Elijah Blue",
    classification: "The dreamer",
    glyph: "◉",
    summary: "A sixteen-year-old searching for the line between memory and inheritance.",
    label: "DIARY FRAGMENT / UNDATED",
    passage: "I woke up with red dust under my fingernails again. Mom says I must have scraped the windowsill in my sleep, but the dust here is pale. In the dream, something beneath West Ridge opened one eye. I don’t think it saw me. I think it remembered me.",
    details: [
      ["Profile", "Sixteen. Restless, observant, and incapable of leaving a mystery alone."],
      ["Earliest dream", "A corridor without doors, warm stone, and a pulse arriving one beat after his own."],
      ["Archive note", "Subject consistently chooses understanding over safety."],
    ],
    references: ["AZEL", "West Ridge", "Synchronization"],
    sealed: "The origin of the recurring trident-shaped mark remains restricted.",
  },
  {
    number: "02",
    title: "AZEL",
    classification: "Living architecture",
    glyph: "⬡",
    summary: "An ancient intelligence sleeping beneath the Arizona desert.",
    label: "PARTIAL ARCHIVE RECORD",
    passage: "AZEL is not a machine housed within the buried architecture. AZEL is the architecture: chamber, pathway, guardian form, memory, and purpose expressed through living stone. What Elijah encounters in dreams may be only the smallest surface of a much older mind.",
    details: [
      ["State", "Dormant systems awakening unevenly beneath West Ridge."],
      ["Guardian form", "A mobile expression of the larger architecture, not its totality."],
      ["Synchronization", "Emerging. Reciprocal. Neither participant remains entirely unchanged."],
    ],
    references: ["Elijah Blue", "Kavanah", "Pre-Edenic Architecture"],
    sealed: "Original directive unavailable. Archive authority exceeds current synchronization.",
  },
  {
    number: "03",
    title: "Hollow Court",
    classification: "Threshold site",
    glyph: "△",
    summary: "A ceremonial ruin where the architecture first begins to answer.",
    label: "LOCATION FILE / PROTOTYPE ZONE",
    passage: "The Hollow Court is a damaged threshold where intention once carried physical weight. Its gates do not respond to force alone. Fractured paths, dormant glyphs, and circling Shardlings suggest the Court is still performing a ceremony whose participants vanished long ago.",
    details: [
      ["Function", "Unknown ceremonial and synchronization threshold."],
      ["Current hazard", "Shardling pursuit through unstable architectural corridors."],
      ["Prototype objective", "Survive the Court, restore the gate response, and reach the sealed passage."],
    ],
    references: ["Shardlings", "Kavanah", "Gate Objective"],
    sealed: "The identity of the Court’s final witness has been removed from this record.",
  },
  {
    number: "04",
    title: "Shardlings",
    classification: "Qlippotic fragments",
    glyph: "✦",
    summary: "Hungry pieces of a broken pattern. Fast, watchful, unfinished.",
    label: "FIELD NOTES / HOSTILE FORM",
    passage: "Shardlings behave less like animals than abandoned instructions. They gather around damaged glyphs, imitate fragments of guardian movement, and pursue coherent sources of resonance. Their aggression appears strongest when a living pattern begins to synchronize nearby.",
    details: [
      ["Behavior", "Circle, observe, isolate, then close distance rapidly."],
      ["Origin class", "Qlippotic residue formed around broken architectural purpose."],
      ["Field guidance", "Read the pause before pursuit. Deliberate movement is safer than panic."],
    ],
    references: ["Qlippot", "Hollow Court", "Synchronization"],
    sealed: "Recovered specimens contain a signal resembling a guardian recall command.",
  },
  {
    number: "05",
    title: "Kavanah",
    classification: "Directed intent",
    glyph: "⌁",
    summary: "Purpose made active—the discipline that gives power its shape.",
    label: "CONCEPT RECORD / MORAL SYSTEM",
    passage: "Kavanah is intention given direction, but intention alone is not enough. AZEL responds to the purpose beneath an action: why power is used, what it protects, and what the wielder is willing to refuse. A stronger bond cannot make a corrupt choice righteous.",
    details: [
      ["Meaning", "Directed purpose made legible to living architecture."],
      ["Moral function", "Choices alter the quality of synchronization, not merely its strength."],
      ["Relationship", "Elijah guides AZEL through intent; AZEL reveals the true shape of that intent."],
    ],
    references: ["AZEL", "Synchronization", "The Last Kavanah"],
    sealed: "The final measure of Kavanah cannot be recorded—only chosen.",
  },
];

export function CodexArchive() {
  const [active, setActive] = useState<Entry | null>(null);

  useEffect(() => {
    if (!active) return;
    const close = (event: KeyboardEvent) => event.key === "Escape" && setActive(null);
    document.addEventListener("keydown", close);
    document.body.classList.add("archive-open");
    return () => {
      document.removeEventListener("keydown", close);
      document.body.classList.remove("archive-open");
    };
  }, [active]);

  return (
    <>
      <div className="codex-grid">
        {entries.map((entry) => (
          <article key={entry.title}>
            <span className="entry-number">{entry.number}</span>
            <p>{entry.classification}</p>
            <h3>{entry.title}</h3>
            <div className="entry-glyph" aria-hidden="true">{entry.glyph}</div>
            <p className="entry-copy">{entry.summary}</p>
            <button type="button" onClick={() => setActive(entry)} aria-label={`Open ${entry.title} Codex entry`}>
              OPEN FILE <span>↗</span>
            </button>
          </article>
        ))}
        <article className="sealed">
          <span className="entry-number">06</span><p>Archive sealed</p><h3>Synchronization</h3>
          <div className="entry-glyph" aria-hidden="true">◈</div>
          <p className="entry-copy">Identity, memory, and architecture entering dangerous resonance.</p>
          <button type="button" disabled>SEALED <span>⌁</span></button>
        </article>
      </div>

      {active && (
        <div className="archive-overlay" role="presentation" onMouseDown={(event) => event.target === event.currentTarget && setActive(null)}>
          <section className="archive-panel" role="dialog" aria-modal="true" aria-labelledby="archive-title">
            <div className="archive-panel-top"><span>BLAZE CODEX / FILE {active.number}</span><button type="button" onClick={() => setActive(null)} aria-label="Close archive file">CLOSE ×</button></div>
            <div className="archive-title-row"><div><p>{active.classification}</p><h2 id="archive-title">{active.title}</h2></div><span aria-hidden="true">{active.glyph}</span></div>
            <p className="archive-label">{active.label}</p>
            <blockquote>{active.passage}</blockquote>
            <div className="archive-details">{active.details.map(([label,copy]) => <div key={label}><small>{label}</small><p>{copy}</p></div>)}</div>
            <div className="archive-references"><small>CROSS-REFERENCES</small><div>{active.references.map((reference) => <span key={reference}>{reference}</span>)}</div></div>
            <div className="archive-sealed"><span>◈</span><div><small>RESTRICTED STRATUM</small><p>{active.sealed}</p></div></div>
          </section>
        </div>
      )}
    </>
  );
}
