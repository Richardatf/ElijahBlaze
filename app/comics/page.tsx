import Link from "next/link";

const comics = [
  ["01","Hollow Court","Opening Arc","Elijah enters a damaged court of living architecture and encounters the first Shardlings."],
  ["02","The First Guardian","AZEL Prequel","Fragments of AZEL’s ancient history before Elijah, without revealing the whole buried civilization."],
  ["03","West Ridge","Grounded Mystery","Survey records, symbols, and disturbances beneath the Arizona desert."],
  ["04","Shardfall","Limited Series","A major rupture in the hidden architecture reaches the surface world."],
  ["05","The Seven Echoes","Anthology","Seven stories from lost chambers, forgotten guardians, and failures in the old world."],
  ["06","Nora","Side Story","Elijah’s changes seen from outside his own mind, through the eyes of the friend who knows him best."],
  ["07","Before the Flame","Prequel","Elijah Blue before full synchronization: family, school, notebooks, curiosity, and the earliest dreams."],
];

export default function ComicsPage(){
 return <main id="main-content">
  <header className="nav-shell"><Link className="brand" href="/"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></Link><nav><Link href="/">Home</Link><Link href="/game">Game</Link><Link href="/books">Books</Link><a href="#slate">Comic Slate</a></nav><Link className="nav-signal" href="/#signal"><span/>SIGNAL LIVE</Link></header>
  <section className="section codex" style={{paddingTop:150}}><div className="section-heading"><div><p className="kicker">ELIJAH BLAZE COMICS</p><h2>STORIES BESIDE,<br/><i>BEFORE & BENEATH.</i></h2></div><p className="lead">The novels carry Elijah’s central journey. The comics widen the universe with places, witnesses, and ancient memories the novels do not need to stop and explain.</p></div>
  <div className="codex-grid" id="slate">{comics.map(([num,title,type,copy],i)=><article key={title}><span className="entry-number">COMIC FILE / {num}</span><p>{type}</p><h3>{title}</h3><div className="entry-glyph">{["◇","◈","⌖","✦","⑦","○","△"][i]}</div><p className="entry-copy">{copy}</p></article>)}</div></section>
  <section className="section studio"><div className="studio-copy"><p className="kicker">COMIC RULE</p><h2>EXPAND.<br/><i>DON’T REPEAT.</i></h2><p className="manifesto">A comic should never exist merely to retell a chapter with pictures.</p><p>Each arc reveals a perspective, event, location, or recovered history that deepens the larger Elijah Blaze universe while remaining readable on its own.</p></div><div className="principles"><p>VISUAL LANGUAGE</p>{[["01","ARCHITECTURE AS CHARACTER"],["02","SYMBOLS WITH CONSEQUENCE"],["03","MYSTERY BEFORE EXPLANATION"],["04","REVELATION THROUGH IMAGE"]].map(x=><div key={x[0]}><span>{x[0]}</span><h3>{x[1]}</h3><b>↗</b></div>)}</div></section>
 </main>
}
