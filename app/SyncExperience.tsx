"use client";

import Image from "next/image";
import { useState } from "react";
import styles from "./SyncExperience.module.css";

const stages = [
  {
    name: "DREAM",
    level: "01",
    title: "Something is dreaming back.",
    copy: "Elijah begins with fragments: stone corridors, impossible silence, and memories that do not belong to him.",
    signal: "FAINT SIGNAL DETECTED",
    quote: "I thought dreams were the one place nobody else could follow you.",
  },
  {
    name: "RESONANCE",
    level: "02",
    title: "Two patterns begin to match.",
    copy: "The sleeping mind of AZEL and Elijah’s dreaming brain begin finding the same rhythm across thousands of years.",
    signal: "FREQUENCY MATCHING",
    quote: "The dream stopped feeling like a place I visited. It started feeling like a place that remembered me.",
  },
  {
    name: "CONTACT",
    level: "03",
    title: "The architecture notices him.",
    copy: "Elijah is no longer merely receiving. His emotions, memories, and intentions begin crossing the same bridge in return.",
    signal: "TWO MINDS IN PHASE",
    quote: "For one second I knew I wasn't alone. The stranger part was knowing it felt the same thing.",
  },
  {
    name: "TRUST",
    level: "04",
    title: "The barrier becomes a choice.",
    copy: "The bond deepens only when Elijah willingly allows AZEL into the parts of himself he would rather keep hidden.",
    signal: "KAVANAH ALIGNMENT",
    quote: "Trust wasn't letting it hear my thoughts. Trust was letting it hear the thought I was hiding.",
  },
  {
    name: "SYMBIOSIS",
    level: "05",
    title: "Neither remains unchanged.",
    copy: "Elijah gives AZEL human memory, instinct, grief, humor, and love. AZEL gives Elijah an ancient architecture through which intention can become action.",
    signal: "GUARDIAN FORM POSSIBLE",
    quote: "I do not command AZEL. AZEL does not command me. We align.",
  },
];

export function SyncExperience() {
  const [active, setActive] = useState(0);
  const stage = stages[active];
  const sync = (active + 1) * 20;

  return (
    <section className={styles.section} id="synchronization">
      <div className={styles.heading}>
        <div>
          <p className={styles.kicker}>LIVE ARCHITECTURE / SYNC LAB</p>
          <h2>ENTER THE<br/><i>RESONANCE.</i></h2>
        </div>
        <p className={styles.lead}>Move through the five stages of Elijah Blue and AZEL becoming something neither could become alone.</p>
      </div>

      <div className={styles.experience} data-stage={active + 1}>
        <div className={styles.visual}>
          <div className={styles.geometry} aria-hidden="true">
            <span className={styles.ringOne}/><span className={styles.ringTwo}/><span className={styles.ringThree}/>
            <span className={styles.axis}/><span className={styles.pulse}/>
          </div>
          <div className={styles.avatar}>
            <Image src="/assets/elijah-blaze-avatar.png" alt="Elijah Blaze guardian form" fill sizes="(max-width: 800px) 82vw, 520px" />
          </div>
          <div className={styles.readout}>
            <span>AZEL // ELIJAH BLUE</span>
            <strong>{sync}%</strong>
            <small>SYNCHRONIZATION</small>
          </div>
        </div>

        <div className={styles.console}>
          <div className={styles.consoleTop}><span>STAGE {stage.level} / 05</span><b>{stage.signal}</b></div>
          <h3>{stage.title}</h3>
          <p>{stage.copy}</p>
          <blockquote>“{stage.quote}”</blockquote>
          <div className={styles.meter}><span style={{width: `${sync}%`}} /></div>
          <div className={styles.stageButtons} role="tablist" aria-label="Synchronization stages">
            {stages.map((item, index) => (
              <button
                key={item.name}
                className={index === active ? styles.active : ""}
                onClick={() => setActive(index)}
                aria-selected={index === active}
                role="tab"
              >
                <span>{item.level}</span>{item.name}
              </button>
            ))}
          </div>
          <div className={styles.statusLine}><span>ARCHITECTURE STATUS</span><b>{active < 4 ? "BECOMING" : "AWAKE"}</b></div>
        </div>
      </div>
    </section>
  );
}
